using Microsoft.EntityFrameworkCore;
using ConcesionarioDDD.Dominio.Entidades;
using ConcesionarioDDD.Dominio.ValueObjects;
using ConcesionarioDDD.SharedKernel;

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

        public DbSet<Vehiculo> Vehiculos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Ignorar DomainEvents en el modelo
            modelBuilder.Ignore<DomainEvent>();

            modelBuilder.Entity<Vehiculo>(entity =>
            {
                entity.HasKey(v => v.Id);
                entity.OwnsMany(v => v.Modificaciones, modificacion =>
                {
                    modificacion.WithOwner().HasForeignKey("VehiculoId");
                    modificacion.Property<int>("Id").ValueGeneratedOnAdd();
                    modificacion.HasKey("Id");
                });

                // Ignorar la colección de eventos de dominio
                entity.Ignore(v => v.DomainEvents);
            });
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