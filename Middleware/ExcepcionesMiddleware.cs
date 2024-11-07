using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MiBackendAPI.Models;

namespace MiBackendAPI.Middleware
{
    public class ExcepcionesMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExcepcionesMiddleware> _logger;

        public ExcepcionesMiddleware(RequestDelegate next, ILogger<ExcepcionesMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext); /
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error en el servidor: {ex.Message}, StackTrace: {ex.StackTrace}");
                await HandleExceptionAsync(httpContext, ex); // Maneja la excepción y responde
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            // Si la excepción es de tipo Validación (ej. ValidationException)
            if (exception is ValidationException validationException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;  // Código 422
                var problemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.UnprocessableEntity,
                    Title = "Error de validación",
                    Detail = validationException.Message,
                    Instance = context.Request.Path
                };

                return context.Response.WriteAsJsonAsync(problemDetails);
            }

            // Si la excepción es de tipo `ArgumentException` o algo relacionado con la lógica de negocio
            if (exception is ArgumentException)
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest; // Código 400
                var problemDetails = new ProblemDetails
                {
                    Status = (int)HttpStatusCode.BadRequest,
                    Title = "Argumento incorrecto",
                    Detail = exception.Message,
                    Instance = context.Request.Path
                };

                return context.Response.WriteAsJsonAsync(problemDetails);
            }

            // Excepciones generales, código 500
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            var generalProblemDetails = new ProblemDetails
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Title = "Error interno del servidor",
                Detail = exception.Message,
                Instance = context.Request.Path
            };

            return context.Response.WriteAsJsonAsync(generalProblemDetails);
        }

    }
}
