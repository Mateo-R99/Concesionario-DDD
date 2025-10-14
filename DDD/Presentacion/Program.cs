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
using ConcesionarioDDD.Dominio.Eventos;
using ConcesionarioDDD.Infraestructura.Events.Handlers;

// Para evitar que la aplicación se cierre inmediatamente
Console.CancelKeyPress += (sender, e) =>
{
    e.Cancel = true;
};

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
    options.ConfigureWarnings(warnings => warnings.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.CoreEventId.NavigationBaseIncluded));
});

// Registrar Memory Cache
builder.Services.AddMemoryCache();

// Registrar servicio de caché
builder.Services.AddSingleton<ICacheService, MemoryCacheService>();

// Register repositories and services
builder.Services.AddSingleton<VehiculoRepositorioEnMemoria>();
builder.Services.AddSingleton<IVehiculoRepositorio>(sp => sp.GetRequiredService<VehiculoRepositorioEnMemoria>());

// Register Unit of Work
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Register Event Dispatcher and Event Handlers
builder.Services.AddScoped<IDomainEventDispatcher, DomainEventDispatcher>();
builder.Services.AddScoped<IEventHandler<VehiculoReservado>, VehiculoReservadoHandler>();
builder.Services.AddScoped<IEventHandler<VehiculoVendido>, VehiculoVendidoHandler>();
builder.Services.AddScoped<IEventHandler<ReservaCancelada>, ReservaCanceladaHandler>();
builder.Services.AddScoped<IEventHandler<ModificacionAgregada>, ModificacionAgregadaHandler>();

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

// Skip HTTPS redirection for development
// app.UseHttpsRedirection();
app.UseCors("AllowAll"); // Habilitar CORS
app.UseAuthorization();
app.MapControllers();

// Inicializar datos solo al arrancar
using (var scope = app.Services.CreateScope())
{
    var vehiculoRepo = scope.ServiceProvider.GetRequiredService<VehiculoRepositorioEnMemoria>();
    vehiculoRepo.InicializarDatos();
}

app.Run();
