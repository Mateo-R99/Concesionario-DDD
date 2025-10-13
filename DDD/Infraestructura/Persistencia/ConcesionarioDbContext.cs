using Microsoft.EntityFrameworkCore;
using ConcesionarioDDD.Dominio.Agregados;
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

        public DbSet<VehiculoAgregado> Vehiculos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configurar el mapeo de entidades aquí si es necesario
        }

        private void SeedData()
        {
            var vehiculos = new[]
            {
                new VehiculoAgregado("Toyota", "Corolla", 2024, "Rojo", 25000m),
                new VehiculoAgregado("Honda", "Civic", 2024, "Azul", 27000m),
                new VehiculoAgregado("Ford", "Mustang", 2024, "Negro", 45000m)
            };

            Vehiculos.AddRange(vehiculos);
            SaveChanges();
        }
    }
}