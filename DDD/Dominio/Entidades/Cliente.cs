using ConcesionarioDDD.SharedKernel;

namespace ConcesionarioDDD.Dominio.Entidades
{
    public class Cliente : Entity<Guid>
    {
        public string Nombre { get; private set; } = string.Empty;
        public string Identificacion { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Telefono { get; private set; } = string.Empty;

        private Cliente() 
        { 
            Nombre = string.Empty;
            Identificacion = string.Empty;
            Email = string.Empty;
            Telefono = string.Empty;
        } // Para EF Core

        public Cliente(string nombre, string identificacion, string email, string telefono)
        {
            Id = Guid.NewGuid();
            Nombre = nombre;
            Identificacion = identificacion;
            Email = email;
            Telefono = telefono;
        }
    }
}