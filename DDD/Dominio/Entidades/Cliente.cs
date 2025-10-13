using ConcesionarioDDD.SharedKernel;

namespace ConcesionarioDDD.Dominio.Entidades
{
    public class Cliente : Entity<Guid>
    {
        public string Nombre { get; private set; }
        public string Identificacion { get; private set; }
        public string Email { get; private set; }
        public string Telefono { get; private set; }

        private Cliente() { } // Para EF Core

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