using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using ConcesionarioDDD.Dominio.Interfaces;
using ConcesionarioDDD.Infraestructura.Persistencia;
using ConcesionarioDDD.Infraestructura.Events;
using ConcesionarioDDD.Aplicacion;
using ConcesionarioDDD.Aplicacion.Handlers;
using ConcesionarioDDD.Aplicacion.Handlers.Interfaces;
using ConcesionarioDDD.SharedKernel.Interfaces;
using FluentValidation;
using FluentValidation.AspNetCore;
using ConcesionarioDDD.Aplicacion.Validators;
using Microsoft.AspNetCore.Mvc;
using ConcesionarioDDD.Infraestructura.Caching;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // Configurar las opciones de serialización JSON
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

// Add FluentValidation
builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters()
    .AddValidatorsFromAssemblyContaining<CrearVehiculoDTOValidator>();

// Configurar comportamiento de validación automática
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = false;
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

// Registrar Memory Cache
builder.Services.AddMemoryCache();

// Registrar servicio de caché
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();

// Register repositories and services
builder.Services.AddScoped<VehiculoRepositorioEnMemoria>();
builder.Services.AddScoped<IVehiculoRepositorio, VehiculoRepositorioConCache>();

// Register Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register Event Dispatcher
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();

// Register Application Handlers
builder.Services.AddScoped<ICrearVehiculoHandler, CrearVehiculoHandler>();
builder.Services.AddScoped<IReservarVehiculoHandler, ReservarVehiculoHandler>();
builder.Services.AddScoped<IAgregarModificacionHandler, AgregarModificacionHandler>();
builder.Services.AddScoped<IVenderVehiculoHandler, VenderVehiculoHandler>();
builder.Services.AddScoped<ICancelarReservaHandler, CancelarReservaHandler>();

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
