using System;

namespace ConcesionarioDDD.Aplicacion.Commands
{
    public class AgregarModificacionCommand
    {
        public Guid VehiculoId { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }

        public AgregarModificacionCommand(Guid vehiculoId, string descripcion, decimal precio)
        {
            VehiculoId = vehiculoId;
            Descripcion = descripcion;
            Precio = precio;
        }
    }
}