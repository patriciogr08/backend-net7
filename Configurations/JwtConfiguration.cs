using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net;
using System.Text;

namespace MiBackendAPI.Configurations
{
    public static class JwtConfiguration
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidAudience = configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(configuration["Jwt:Key"]))
                    };

                    // Evento para manejar cuando falla la autenticación
                    options.Events = new JwtBearerEvents
                    {
                        OnAuthenticationFailed = context =>
                        {
                            if (context.Exception is SecurityTokenExpiredException || context.Exception is SecurityTokenInvalidSignatureException)
                            {
                                // Aquí puedes hacer un log si es necesario o agregar encabezados personalizados
                                context.Response.Headers.Add("Token-Expired", "true");
                            }
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            // Aquí puedes personalizar la respuesta de error cuando falta o es inválido el token
                            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                            context.Response.ContentType = "application/json";

                            var problemDetails = new ProblemDetails
                            {
                                Title = "No autorizado",
                                Status = (int)HttpStatusCode.Unauthorized,
                                Detail = "Se requiere un token de autorización.",
                                Instance = context.Request.Path
                            };

                            return context.Response.WriteAsJsonAsync(problemDetails);
                        }
                    };
                });

            return services;
        }
    }
}
