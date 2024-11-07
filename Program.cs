using MiBackendAPI.BusinessLogic;
using MiBackendAPI.Configurations;
using MiBackendAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// Llama al m�todo AddDatabaseConfiguration
builder.Services.AddDatabaseConfiguration(builder.Configuration);

//Seccion para agregar los servicios creados
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<IRolService, RolService>();


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddJwtAuthentication(builder.Configuration);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Middleware de autenticaci�n y autorizaci�n
app.UseAuthentication();  // Habilitar autenticaci�n
app.UseAuthorization();   // Habilitar autorizaci�n

app.UseMiddleware<ExcepcionesMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
