using Microsoft.AspNetCore.Mvc;
using ConcesionarioDDD.Dominio.Entidades;
using ConcesionarioDDD.Dominio.Interfaces;
using ConcesionarioDDD.Aplicacion.DTOs;

namespace ConcesionarioDDD.Presentacion.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiculosController : ControllerBase
    {
        private readonly IVehiculoRepositorio _vehiculoRepositorio;

        public VehiculosController(IVehiculoRepositorio vehiculoRepositorio)
        {
            _vehiculoRepositorio = vehiculoRepositorio;
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
                }).ToList()
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
                }).ToList()
            };

            return Ok(vehiculoDto);
        }

        [HttpPost]
        public async Task<ActionResult<VehiculoDTO>> CrearVehiculo([FromBody] CrearVehiculoDTO dto)
        {
            var vehiculo = new Vehiculo(dto.Marca, dto.Modelo, dto.Año, dto.Color, dto.PrecioBase);
            await _vehiculoRepositorio.AgregarAsync(vehiculo);

            return CreatedAtAction(nameof(ObtenerPorId), new { id = vehiculo.Id }, vehiculo);
        }

        [HttpPost("{id}/modificaciones")]
        public async Task<IActionResult> AgregarModificacion(Guid id, [FromBody] ModificacionDTO dto)
        {
            var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(id);
            if (vehiculo == null)
                return NotFound();

            try
            {
                vehiculo.AgregarModificacion(dto.Descripcion, dto.Precio);
                await _vehiculoRepositorio.ActualizarAsync(vehiculo);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("reservar/{id}")]
        public async Task<IActionResult> ReservarVehiculo(Guid id)
        {
            var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(id);
            if (vehiculo == null)
                return NotFound();

            try
            {
                vehiculo.MarcarComoReservado();
                await _vehiculoRepositorio.ActualizarAsync(vehiculo);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("vender/{id}")]
        public async Task<IActionResult> VenderVehiculo(Guid id)
        {
            var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(id);
            if (vehiculo == null)
                return NotFound();

            try
            {
                vehiculo.MarcarComoVendido();
                await _vehiculoRepositorio.ActualizarAsync(vehiculo);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("cancelar-reserva/{id}")]
        public async Task<IActionResult> CancelarReserva(Guid id)
        {
            var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(id);
            if (vehiculo == null)
                return NotFound();

            try
            {
                vehiculo.CancelarReserva();
                await _vehiculoRepositorio.ActualizarAsync(vehiculo);
                return Ok();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}