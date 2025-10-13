# Concesionario DDD

Proyecto de ejemplo que ilustra un diseño basado en Domain-Driven Design (DDD) para un concesionario de vehículos.

Este repositorio contiene una aplicación en .NET (ASP.NET Core) organizada por capas/árbols DDD: Dominio, Aplicación, Infraestructura y Presentación. Usa Entity Framework Core con una base de datos en memoria para facilitar el desarrollo y pruebas.

## Contenido principal

- `DDD/Presentacion` - Proyecto ASP.NET Core que expone la API (Program.cs, controladores, configuración). Incluye Swagger para exploración.
- `DDD/Dominio` - Entidades, Value Objects e interfaces de repositorios y servicios del dominio (modelo de negocio). Contiene las entidades `Vehiculo`, `Cliente`, `Cotizacion`, y objetos de valor como `Precio`, `Modificacion`.
- `DDD/Aplicacion` - Casos de uso (handlers) y DTOs que orquestan la lógica de aplicación.
- `DDD/Infraestructura` - Implementaciones concretas para persistencia (EF Core DbContext y repositorios). Hay un repositorio `VehiculoRepositorio` y `ConcesionarioDbContext` con datos de semilla.
- `DDD/SharedKernel` - Clases base reutilizables como `Entity`, `ValueObject` y el tipo `Result` para resultados de operaciones.

## Requisitos

- .NET SDK 7.0 o superior (instalado en el sistema)
- Windows / macOS / Linux - el proyecto usa EF Core InMemory, por lo que no necesita una base de datos externa.

Comprueba la versión instalada con:

```powershell
dotnet --version
```

## Ejecutar la API (desarrollo)

1. Abre una terminal en la carpeta `DDD/Presentacion`:

```powershell
cd DDD/Presentacion
```

2. Ejecuta la aplicación:

```powershell
dotnet run
```

3. Por defecto la aplicación expone Swagger en modo Development. Abre en tu navegador la URL mostrada en la salida (normalmente `https://localhost:5001/swagger` o `http://localhost:5000/swagger`).

4. La base de datos es en memoria y viene pre-poblada con algunos vehículos de ejemplo (Toyota Corolla, Honda Civic, Ford Mustang).

## Endpoints (orientativo)

Los controladores están en `DDD/Presentacion/Controllers` (si no existen, revisa la carpeta). Basado en la estructura del proyecto, los endpoints esperados son para gestionar vehículos, cotizaciones y ventas. Ejemplos posibles:

- GET /api/vehiculos - obtener todos los vehículos
- GET /api/vehiculos/{id} - obtener vehículo por id
- POST /api/vehiculos - crear un vehículo
- PUT /api/vehiculos/{id} - actualizar un vehículo

Usa Swagger para ver los endpoints exactos y los DTOs requeridos.

## Estructura del código y decisiones de diseño

- Domain-Driven Design (DDD): el núcleo de negocio está en `DDD/Dominio`. Las entidades son ricas en comportamiento (por ejemplo, `Vehiculo` puede reservarse, marcarse como vendido, agregar modificaciones).
- Separación de capas: los handlers de `Aplicacion` actúan como orquestadores de casos de uso; `Infraestructura` contiene implementaciones concretas (repositorios y DbContext).
- SharedKernel: utilidades y tipos compartidos (por ejemplo `Result<T>`) que modelan el resultado de operaciones.
- Persistencia: se usa EF Core con proveedor InMemory para simplificar el arranque y pruebas.

## Cómo contribuir

- Abrir un issue describiendo bug o feature.
- Crear una rama por feature/bugfix: `feature/nombre` o `fix/nombre`.
- Hacer un PR hacia `main` con descripción clara de cambios.

## Próximos pasos y mejoras sugeridas

- Añadir pruebas unitarias e integración (xUnit o NUnit). Incluir pruebas para handlers del proyecto `Aplicacion` y para repositorios.
- Añadir pruebas de contrato para la API y CI (GitHub Actions).
- Sustituir la base de datos InMemory por SQL Server o PostgreSQL para entornos de staging/producción y agregar migraciones EF Core.
- Implementar validaciones y manejo de errores más robusto en la capa de presentación.
- Documentar los contratos DTO concretos en `DDD/Aplicacion/DTOs` y ejemplos de petición/respuesta.

## Notas rápidas

- Proyecto de ejemplo educativo: la implementación actual prioriza la claridad del diseño DDD sobre detalles completos de seguridad, escalabilidad o rendimiento.
- Revisa `Program.cs` en `DDD/Presentacion` para ver la configuración del servicio, CORS y el uso de Swagger.

Si quieres, puedo:

- Generar un archivo README más detallado en inglés y español.
- Añadir ejemplos concretos de requests/responses basados en los controladores actuales.
- Crear pruebas unitarias iniciales para un handler.

Indícame qué prefieres y continúo con ello.
