using System;
using System.Threading.Tasks;
using ConcesionarioDDD.Dominio.Interfaces;
using ConcesionarioDDD.SharedKernel;
using ConcesionarioDDD.Dominio.Entidades;

namespace ConcesionarioDDD.Aplicacion.UseCases
{
    public class EntregarVehiculoHandler
    {
        private readonly IVentasRepositorio _ventaRepositorio;
        private readonly IVehiculoRepositorio _vehiculoRepositorio;
        private readonly INotificacionCliente _notificacionCliente;
        private readonly IServicioPostVenta _servicioPostVenta;

        public EntregarVehiculoHandler(
            IVentasRepositorio ventaRepositorio,
            IVehiculoRepositorio vehiculoRepositorio,
            INotificacionCliente notificacionCliente,
            IServicioPostVenta servicioPostVenta)
        {
            _ventaRepositorio = ventaRepositorio ?? throw new ArgumentNullException(nameof(ventaRepositorio));
            _vehiculoRepositorio = vehiculoRepositorio ?? throw new ArgumentNullException(nameof(vehiculoRepositorio));
            _notificacionCliente = notificacionCliente ?? throw new ArgumentNullException(nameof(notificacionCliente));
            _servicioPostVenta = servicioPostVenta ?? throw new ArgumentNullException(nameof(servicioPostVenta));
        }

        public async Task<Result> Handle(EntregarVehiculoRequest request)
        {
            if (request == null)
                return Result.Fail("Request no puede ser null");

            var venta = await _ventaRepositorio.ObtenerPorIdAsync(request.VentaId);
            if (venta == null)
                return Result.Fail("Venta no encontrada");

            var vehiculo = await _vehiculoRepositorio.ObtenerPorIdAsync(venta.VehiculoId);
            if (vehiculo == null)
                return Result.Fail("Vehículo no encontrado");

            if (!venta.EstaPagada)
                return Result.Fail("La venta no está pagada completamente");

            var resultado = venta.MarcarComoEntregado();
            if (!resultado.IsSuccess)
                return resultado;

            vehiculo.MarcarComoEntregado();

            await _ventaRepositorio.ActualizarAsync(venta);
            await _vehiculoRepositorio.ActualizarAsync(vehiculo);

            // Registrar en servicio post-venta
            await _servicioPostVenta.RegistrarEntregaAsync(venta.Id, request.FechaEntrega);

            // Notificar al cliente
            await _notificacionCliente.EnviarNotificacionVehiculoEntregadoAsync(venta);

            return Result.Success();
        }
    }

    public class EntregarVehiculoRequest
    {
        public Guid VentaId { get; set; }
        public DateTime FechaEntrega { get; set; }
    }
}