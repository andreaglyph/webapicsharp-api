// Program.cs - Actualizado con nueva estructura de Abstracciones

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using webapicsharp.Repositorios.Abstracciones;
using webapicsharp.Repositorios;

// Crea el "builder": punto de inicio para configurar servicios y la aplicación.
var builder = WebApplication.CreateBuilder(args);

// ---------------------------------------------------------
// CONFIGURACIÓN (OCP: se puede extender sin tocar la lógica)
// ---------------------------------------------------------
builder.Configuration.AddJsonFile(
    "tablasprohibidas.json",
    optional: true,
    reloadOnChange: true
);

// ---------------------------------------------------------
// SERVICIOS (SRP: solo registro, sin lógica de negocio aquí)
// ---------------------------------------------------------
builder.Services.AddControllers();

// CORS (Cross-Origin Resource Sharing) Intercambio de recursos de origen cruzado
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular",
        policy => policy.WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
});

// Agrega caché en memoria y sesión HTTP ligera (opcional)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(opciones =>
{
    opciones.IdleTimeout = TimeSpan.FromMinutes(30);
    opciones.Cookie.HttpOnly = true;
    opciones.Cookie.IsEssential = true;
});

// Agrega servicios para exponer documentación Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// -----------------------------------------------------------------
// NOTA DIP: el registro de interfaces → implementaciones irá aquí
// -----------------------------------------------------------------
builder.Services.AddScoped<webapicsharp.Servicios.Abstracciones.IServicioCrud,
                           webapicsharp.Servicios.ServicioCrud>();

builder.Services.AddSingleton<webapicsharp.Servicios.Abstracciones.IProveedorConexion,
                              webapicsharp.Servicios.Conexion.ProveedorConexion>();

builder.Services.AddScoped<webapicsharp.Servicios.Abstracciones.IServicioConsultas,
                           webapicsharp.Servicios.ServicioConsultas>();

var proveedorBD = builder.Configuration.GetValue<string>("DatabaseProvider") ?? "SqlServer";

switch (proveedorBD.ToLower())
{
    case "postgres":
        builder.Services.AddScoped<IRepositorioLecturaTabla, RepositorioLecturaPostgreSQL>();
        builder.Services.AddScoped<IRepositorioConsultas, RepositorioConsultasPostgreSQL>();
        break;
    case "mariadb":
        builder.Services.AddScoped<IRepositorioLecturaTabla, RepositorioLecturaMariaDB>();
        builder.Services.AddScoped<IRepositorioConsultas, RepositorioConsultasMariaDB>();
        break;
    case "mysql":
        // pendiente si se usa MySQL
        break;
    case "sqlserver":
    case "sqlserverexpress":
    case "localdb":
    default:
        builder.Services.AddScoped<webapicsharp.Repositorios.Abstracciones.IRepositorioLecturaTabla,
                                   webapicsharp.Repositorios.RepositorioLecturaSqlServer>();

        builder.Services.AddScoped<webapicsharp.Repositorios.Abstracciones.IRepositorioConsultas,
                                   webapicsharp.Repositorios.RepositorioConsultasSqlServer>();
        break;
}

// ---------------------------------------------------------
// Construir la app (una sola vez)
// ---------------------------------------------------------
var app = builder.Build();

// ---------------------------------------------------------
// MIDDLEWARE (orden importa: se ejecuta de arriba hacia abajo)
// ---------------------------------------------------------
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "webapicsharp v1");
    c.RoutePrefix = "swagger";
});

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseSession();
app.UseAuthorization();
app.MapControllers();

// Arranca la aplicación y queda escuchando solicitudes HTTP
app.Run();
