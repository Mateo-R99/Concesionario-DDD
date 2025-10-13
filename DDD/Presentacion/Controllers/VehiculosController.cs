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
                PrecioBase = v.PrecioBase.MontoBase,
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
                PrecioBase = vehiculo.PrecioBase.MontoBase,
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

        [HttpPost("reservar/{id}")]
        public async Task<IActionResult> ReservarVehiculo(Guid id)
        {
            var command = new ReservarVehiculoCommand(id);

            var result = await _reservarVehiculoHandler.Handle(command);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok();
        }

        [HttpPost("vender/{id}")]
        public async Task<IActionResult> VenderVehiculo(Guid id)
        {
            var command = new VenderVehiculoCommand(id);

            var result = await _venderVehiculoHandler.Handle(command);

            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(new { PrecioFinal = result.Value });
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