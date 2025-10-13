using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using ConcesionarioDDD.Dominio.Interfaces;
using ConcesionarioDDD.Infraestructura.Persistencia;
using ConcesionarioDDD.Aplicacion;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configurar las opciones de serialización JSON
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

// Configure Entity Framework Core with in-memory database
builder.Services.AddDbContext<ConcesionarioDbContext>(options =>
{
    options.UseInMemoryDatabase("ConcesionarioDB");
    // Desactivar el tracking por defecto para mejor rendimiento
    options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
});

// Register repositories and services
builder.Services.AddScoped<IVehiculoRepositorio, VehiculoRepositorio>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowAll"); // Habilitar CORS
app.UseAuthorization();
app.MapControllers();

app.Run();
