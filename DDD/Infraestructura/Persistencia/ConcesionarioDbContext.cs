using Microsoft.EntityFrameworkCore;
using ConcesionarioDDD.Dominio.Entidades;
using ConcesionarioDDD.Dominio.ValueObjects;

namespace ConcesionarioDDD.Infraestructura.Persistencia
{
    public class ConcesionarioDbContext : DbContext
    {
        public ConcesionarioDbContext(DbContextOptions<ConcesionarioDbContext> options)
            : base(options)
        {
            Database.EnsureCreated();
            if (!Vehiculos.Any())
            {
                SeedData();
            }
        }

        public DbSet<Vehiculo> Vehiculos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar el mapeo de entidades aquí si es necesario
        }

        private void SeedData()
        {
            var vehiculos = new[]
            {
                new Vehiculo("Toyota", "Corolla", 2024, "Rojo", 25000m),
                new Vehiculo("Honda", "Civic", 2024, "Azul", 27000m),
                new Vehiculo("Ford", "Mustang", 2024, "Negro", 45000m)
            };

            Vehiculos.AddRange(vehiculos);
            SaveChanges();
        }
    }
}