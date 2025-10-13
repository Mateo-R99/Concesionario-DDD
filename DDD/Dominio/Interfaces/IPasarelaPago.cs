using System;
using System.Threading.Tasks;
using ConcesionarioDDD.Dominio.Entidades;

namespace ConcesionarioDDD.Dominio.Interfaces
{
    public interface IPasarelaPago
    {
        Task<bool> ValidarPagoAsync(decimal monto, string metodoPago, string referencia);
        Task<string> ProcesarPagoAsync(decimal monto, string metodoPago);
    }
}