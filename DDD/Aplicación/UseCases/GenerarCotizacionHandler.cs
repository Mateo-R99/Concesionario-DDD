using System;
using System.Threading.Tasks;
using ConcesionarioDDD.Dominio.Interfaces;
using ConcesionarioDDD.Dominio.Entidades;
using ConcesionarioDDD.Dominio.ValueObjects;
using ConcesionarioDDD.SharedKernel;
using ConcesionarioDDD.Aplicacion.DTOs;

namespace ConcesionarioDDD.Aplicacion.UseCases
{
    public class GenerarCotizacionHandler
    {
        private readonly IVehiculoRepositorio _vehiculoRepositorio;
        private readonly ICotizacionRepositorio _cotizacionRepositorio;

        public GenerarCotizacionHandler(
            IVehiculoRepositorio vehiculoRepositorio,
            ICotizacionRepositorio cotizacionRepositorio)
        {
            _vehiculoRepositorio = vehiculoRepositorio ?? throw new ArgumentNullException(nameof(vehiculoRepositorio));
            _cotizacionRepositorio = cotizacionRepositorio ?? throw new ArgumentNullException(nameof(cotizacionRepositorio));
        }

        public async Task<Result<Cotizacion>> Handle(GenerarCotizacionRequest request)
        {
            if (request == null)
                return Result<Cotizacion>.Fail("Request no puede ser null");

            var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(request.VehiculoId);
            if (vehiculo == null)
                return Result<Cotizacion>.Fail("Vehículo no encontrado");

            if (vehiculo.Estado != EstadoVehiculo.Disponible)
                return Result<Cotizacion>.Fail("El vehículo no está disponible");

            var cotizacion = new Cotizacion(vehiculo.Id, vehiculo.PrecioBase, request.DiasVigencia);

            // Agregar extras
            if (request.Extras != null)
            {
                foreach (var extra in request.Extras)
                {
                    if (extra == null) continue;
                    var precioExtra = new Precio(extra.Precio);
                    cotizacion.AgregarExtra(extra.Descripcion, precioExtra);
                }
            }

            // Aplicar descuento
            if (request.PorcentajeDescuento.HasValue && request.PorcentajeDescuento.Value > 0)
            {
                cotizacion.AplicarDescuento(request.PorcentajeDescuento.Value);
            }

            await _cotizacionRepositorio.AgregarAsync(cotizacion);

            return Result<Cotizacion>.Ok(cotizacion);
        }
    }

    public class GenerarCotizacionRequest
    {
        public Guid VehiculoId { get; set; }
        public int DiasVigencia { get; set; }
        public List<ExtraRequest>? Extras { get; set; }
        public decimal? PorcentajeDescuento { get; set; }
    }

    public class ExtraRequest
    {
        public string Descripcion { get; set; } = string.Empty;
        public decimal Precio { get; set; }
    }
}