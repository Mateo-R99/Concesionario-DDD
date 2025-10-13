using System;
using System.Threading.Tasks;
using ConcesionarioDDD.Dominio.Interfaces;
using ConcesionarioDDD.SharedKernel;

namespace ConcesionarioDDD.Aplicacion.UseCases
{
    public class RegistrarPagoHandler
    {
        private readonly IVentasRepositorio _ventaRepositorio;
        private readonly INotificacionCliente _notificacionCliente;
        private readonly IPasarelaPago _pasarelaPago;

        public RegistrarPagoHandler(
            IVentasRepositorio ventaRepositorio,
            INotificacionCliente notificacionCliente,
            IPasarelaPago pasarelaPago)
        {
            _ventaRepositorio = ventaRepositorio ?? throw new ArgumentNullException(nameof(ventaRepositorio));
            _notificacionCliente = notificacionCliente ?? throw new ArgumentNullException(nameof(notificacionCliente));
            _pasarelaPago = pasarelaPago ?? throw new ArgumentNullException(nameof(pasarelaPago));
        }

        public async Task<Result> Handle(RegistrarPagoRequest request)
        {
            if (request == null)
                return Result.Fail("Request no puede ser null");

            var venta = await _ventaRepositorio.ObtenerPorIdAsync(request.VentaId);
            if (venta == null)
                return Result.Fail("Venta no encontrada");

            // Validar el pago con la pasarela externa
            var pagoValidado = await _pasarelaPago.ValidarPagoAsync(
                venta.PrecioFinal.Monto,
                request.MetodoPago,
                request.Referencia);

            if (!pagoValidado)
                return Result.Fail("El pago no pudo ser validado");

            var resultado = venta.RegistrarPago();
            if (!resultado.IsSuccess)
                return resultado;

            await _ventaRepositorio.ActualizarAsync(venta);
            await _notificacionCliente.EnviarNotificacionPagoRecibidoAsync(venta);

            return Result.Success();
        }
    }

    public class RegistrarPagoRequest
    {
        public Guid VentaId { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public string Referencia { get; set; } = string.Empty;
    }
}