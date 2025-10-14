using Microsoft.AspNetCore.Mvc;
using ConcesionarioDDD.Aplicacion.DTOs;
using ConcesionarioDDD.Aplicacion.Commands;
using ConcesionarioDDD.Aplicacion.Handlers.Interfaces;
using ConcesionarioDDD.Dominio.Interfaces;

namespace ConcesionarioDDD.Presentacion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiculosController : ControllerBase
    {
        private readonly IVehiculoRepositorio _vehiculoRepositorio;
        private readonly ICrearVehiculoHandler _crearVehiculoHandler;
        private readonly IReservarVehiculoHandler _reservarVehiculoHandler;
        private readonly IAgregarModificacionHandler _agregarModificacionHandler;
        private readonly IVenderVehiculoHandler _venderVehiculoHandler;
        private readonly ICancelarReservaHandler _cancelarReservaHandler;

        public VehiculosController(
            IVehiculoRepositorio vehiculoRepositorio,
            ICrearVehiculoHandler crearVehiculoHandler,
            IReservarVehiculoHandler reservarVehiculoHandler,
            IAgregarModificacionHandler agregarModificacionHandler,
            IVenderVehiculoHandler venderVehiculoHandler,
            ICancelarReservaHandler cancelarReservaHandler)
        {
            _vehiculoRepositorio = vehiculoRepositorio;
            _crearVehiculoHandler = crearVehiculoHandler;
            _reservarVehiculoHandler = reservarVehiculoHandler;
            _agregarModificacionHandler = agregarModificacionHandler;
            _venderVehiculoHandler = venderVehiculoHandler;
            _cancelarReservaHandler = cancelarReservaHandler;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VehiculoDTO>>> ObtenerTodos()
        {
            var vehiculos = await _vehiculoRepositorio.ObtenerTodosAsync();
            var vehiculosDto = vehiculos.Select(v => new VehiculoDTO
            {
                Id = v.Id,
                Marca = v.Marca,
                Modelo = v.Modelo,
                Año = v.Año,
                Color = v.Color,
                PrecioBase = v.PrecioBase,
                Estado = v.Estado.ToString(),
                Modificaciones = v.Modificaciones?.Select(m => new ModificacionDTO
                {
                    Descripcion = m.Descripcion,
                    Precio = m.Precio
                }).ToList() ?? new List<ModificacionDTO>()
            });

            return Ok(vehiculosDto);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VehiculoDTO>> ObtenerPorId(Guid id)
        {
            var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(id);
            if (vehiculo == null)
                return NotFound();

            var vehiculoDto = new VehiculoDTO
            {
                Id = vehiculo.Id,
                Marca = vehiculo.Marca,
                Modelo = vehiculo.Modelo,
                Año = vehiculo.Año,
                Color = vehiculo.Color,
                PrecioBase = vehiculo.PrecioBase,
                Estado = vehiculo.Estado.ToString(),
                Modificaciones = vehiculo.Modificaciones?.Select(m => new ModificacionDTO
                {
                    Descripcion = m.Descripcion,
                    Precio = m.Precio
                }).ToList() ?? new List<ModificacionDTO>()
            };

            return Ok(vehiculoDto);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CrearVehiculo([FromBody] CrearVehiculoDTO dto)
        {
            var command = new CrearVehiculoCommand(
                dto.Marca,
                dto.Modelo,
                dto.Año,
                dto.Color,
                dto.PrecioBase
            );

            var result = await _crearVehiculoHandler.Handle(command);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return CreatedAtAction(nameof(ObtenerPorId), new { id = result.Value }, result.Value);
        }

        [HttpGet("{id}/modificaciones")]
        public async Task<ActionResult<IEnumerable<ModificacionDTO>>> ObtenerModificaciones(Guid id)
        {
            var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(id);
            if (vehiculo == null)
                return NotFound();

            var modificaciones = vehiculo.Modificaciones?.Select(m => new ModificacionDTO
            {
                Descripcion = m.Descripcion,
                Precio = m.Precio
            }).ToList() ?? new List<ModificacionDTO>();

            return Ok(modificaciones);
        }

        [HttpPost("{id}/modificaciones")]
        public async Task<IActionResult> AgregarModificacion(Guid id, [FromBody] ModificacionDTO dto)
        {
            var command = new AgregarModificacionCommand(
                id,
                dto.Descripcion,
                dto.Precio
            );

            var result = await _agregarModificacionHandler.Handle(command);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok();
        }

        [HttpGet("reservados")]
        public async Task<ActionResult<IEnumerable<VehiculoDTO>>> ObtenerVehiculosReservados()
        {
            var vehiculos = await _vehiculoRepositorio.ObtenerTodosAsync();
            var vehiculosReservados = vehiculos
                .Where(v => v.Estado == Dominio.EstadoVehiculo.Reservado)
                .Select(v => new VehiculoDTO
                {
                    Id = v.Id,
                    Marca = v.Marca,
                    Modelo = v.Modelo,
                    Año = v.Año,
                    Color = v.Color,
                    PrecioBase = v.PrecioBase,
                    Estado = v.Estado.ToString(),
                    Modificaciones = v.Modificaciones?.Select(m => new ModificacionDTO
                    {
                        Descripcion = m.Descripcion,
                        Precio = m.Precio
                    }).ToList() ?? new List<ModificacionDTO>()
                });

            return Ok(vehiculosReservados);
        }

        [HttpPost("reservar/{id}")]
        public async Task<IActionResult> ReservarVehiculo(Guid id)
        {
            // Primero verificamos si el vehículo existe y está disponible
            var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(id);
            if (vehiculo == null)
                return NotFound("Vehículo no encontrado");

            var clienteId = Guid.NewGuid(); // En un sistema real, esto vendría del token de autenticación
            var command = new ReservarVehiculoCommand(id, clienteId);

            var result = await _reservarVehiculoHandler.Handle(command);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            // Obtenemos el vehículo actualizado después de la reserva
            vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(id);

            var vehiculoDto = new VehiculoDTO
            {
                Id = vehiculo.Id,
                Marca = vehiculo.Marca,
                Modelo = vehiculo.Modelo,
                Año = vehiculo.Año,
                Color = vehiculo.Color,
                PrecioBase = vehiculo.PrecioBase,
                Estado = vehiculo.Estado.ToString(),
                Modificaciones = vehiculo.Modificaciones?.Select(m => new ModificacionDTO
                {
                    Descripcion = m.Descripcion,
                    Precio = m.Precio
                }).ToList() ?? new List<ModificacionDTO>()
            };

            return Ok(new
            {
                Mensaje = "Vehículo reservado exitosamente",
                Vehiculo = vehiculoDto,
                ClienteId = clienteId
            });
        }

        [HttpGet("vender/{id}")]
        public async Task<ActionResult<VehiculoDTO>> ObtenerVehiculoParaVender(Guid id)
        {
            var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(id);
            if (vehiculo == null)
                return NotFound("Vehículo no encontrado");

            decimal precioTotal = vehiculo.PrecioBase;
            if (vehiculo.Modificaciones != null)
            {
                precioTotal += vehiculo.Modificaciones.Sum(m => m.Precio);
            }

            var vehiculoDto = new VehiculoDTO
            {
                Id = vehiculo.Id,
                Marca = vehiculo.Marca,
                Modelo = vehiculo.Modelo,
                Año = vehiculo.Año,
                Color = vehiculo.Color,
                PrecioBase = vehiculo.PrecioBase,
                Estado = vehiculo.Estado.ToString(),
                Modificaciones = vehiculo.Modificaciones?.Select(m => new ModificacionDTO
                {
                    Descripcion = m.Descripcion,
                    Precio = m.Precio
                }).ToList() ?? new List<ModificacionDTO>()
            };

            return Ok(new
            {
                Vehiculo = vehiculoDto,
                PrecioTotal = precioTotal,
                DisponibleParaVenta = vehiculo.Estado == Dominio.EstadoVehiculo.Disponible || vehiculo.Estado == Dominio.EstadoVehiculo.Reservado
            });
        }

        [HttpPost("vender/{id}")]
        public async Task<IActionResult> VenderVehiculo(Guid id)
        {
            var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(id);
            if (vehiculo == null)
                return NotFound("Vehículo no encontrado");

            if (vehiculo.Estado != Dominio.EstadoVehiculo.Disponible && vehiculo.Estado != Dominio.EstadoVehiculo.Reservado)
                return BadRequest("El vehículo no está disponible para la venta");

            var command = new VenderVehiculoCommand(id);
            var result = await _venderVehiculoHandler.Handle(command);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            // Obtenemos el vehículo actualizado después de la venta
            vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(id);
            if (vehiculo == null)
                return Ok(new { Mensaje = "Vehículo vendido exitosamente", PrecioFinal = result.Value });

            decimal precioTotal = result.Value;
            var vehiculoDto = new VehiculoDTO
            {
                Id = vehiculo.Id,
                Marca = vehiculo.Marca,
                Modelo = vehiculo.Modelo,
                Año = vehiculo.Año,
                Color = vehiculo.Color,
                PrecioBase = vehiculo.PrecioBase,
                Estado = vehiculo.Estado.ToString(),
                Modificaciones = vehiculo.Modificaciones?.Select(m => new ModificacionDTO
                {
                    Descripcion = m.Descripcion,
                    Precio = m.Precio
                }).ToList() ?? new List<ModificacionDTO>()
            };

            return Ok(new
            {
                Mensaje = "Vehículo vendido exitosamente",
                Vehiculo = vehiculoDto,
                PrecioFinal = precioTotal
            });
        }

        [HttpGet("reservar/{id}")]
        public async Task<ActionResult<VehiculoDTO>> ObtenerVehiculoParaReservar(Guid id)
        {
            var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(id);
            if (vehiculo == null)
                return NotFound("Vehículo no encontrado");

            var vehiculoDto = new VehiculoDTO
            {
                Id = vehiculo.Id,
                Marca = vehiculo.Marca,
                Modelo = vehiculo.Modelo,
                Año = vehiculo.Año,
                Color = vehiculo.Color,
                PrecioBase = vehiculo.PrecioBase,
                Estado = vehiculo.Estado.ToString(),
                Modificaciones = vehiculo.Modificaciones?.Select(m => new ModificacionDTO
                {
                    Descripcion = m.Descripcion,
                    Precio = m.Precio
                }).ToList() ?? new List<ModificacionDTO>()
            };

            return Ok(vehiculoDto);
        }

        [HttpPost("cancelar-reserva/{id}")]
        public async Task<IActionResult> CancelarReserva(Guid id)
        {
            var command = new CancelarReservaCommand(id);

            var result = await _cancelarReservaHandler.Handle(command);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok();
        }
    }
}