using System;
using System.Threading.Tasks;
using ConcesionarioDDD.Dominio.Interfaces;
using ConcesionarioDDD.SharedKernel;
using ConcesionarioDDD.Dominio.Entidades;

namespace ConcesionarioDDD.Aplicacion.UseCases
{
    public class ConfirmarVentaHandler
    {
        private readonly IVentasRepositorio _ventaRepositorio;
        private readonly IVehiculoRepositorio _vehiculoRepositorio;
        private readonly ICotizacionRepositorio _cotizacionRepositorio;
        private readonly INotificacionCliente _notificacionCliente;

        public ConfirmarVentaHandler(
            IVentasRepositorio ventaRepositorio,
            IVehiculoRepositorio vehiculoRepositorio,
            ICotizacionRepositorio cotizacionRepositorio,
            INotificacionCliente notificacionCliente)
        {
            _ventaRepositorio = ventaRepositorio ?? throw new ArgumentNullException(nameof(ventaRepositorio));
            _vehiculoRepositorio = vehiculoRepositorio ?? throw new ArgumentNullException(nameof(vehiculoRepositorio));
            _cotizacionRepositorio = cotizacionRepositorio ?? throw new ArgumentNullException(nameof(cotizacionRepositorio));
            _notificacionCliente = notificacionCliente ?? throw new ArgumentNullException(nameof(notificacionCliente));
        }

        public async Task<Result<Venta>> Handle(ConfirmarVentaRequest request)
        {
            if (request == null)
                return Result<Venta>.Fail("Request no puede ser null");

            var cotizacion = await _cotizacionRepositorio.ObtenerPorIdAsync(request.CotizacionId);
            if (cotizacion == null)
                return Result<Venta>.Fail("Cotización no encontrada");

            var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(cotizacion.VehiculoId);
            if (vehiculo == null)
                return Result<Venta>.Fail("Vehículo no encontrado");

            if (vehiculo.Estado != EstadoVehiculo.Disponible)
                return Result<Venta>.Fail("El vehículo ya no está disponible");

            var venta = new Venta(
                cotizacion.Id,
                request.ClienteId,
                vehiculo.Id,
                cotizacion.PrecioTotal,
                request.CondicionesPago);

            vehiculo.MarcarComoVendido();

            await _ventaRepositorio.AgregarAsync(venta);
            await _vehiculoRepositorio.ActualizarAsync(vehiculo);

            await _notificacionCliente.EnviarNotificacionVentaConfirmadaAsync(venta);

            return Result<Venta>.Ok(venta);
        }
    }

    public class ConfirmarVentaRequest
    {
        public Guid CotizacionId { get; set; }
        public Guid ClienteId { get; set; }
        public CondicionesPago CondicionesPago { get; set; }
    }
}