namespace ConcesionarioDDD.SharedKernel
{
    public abstract class DomainEvent
    {
        public Guid EventoId { get; }
        public DateTime OcurridoEn { get; }

        protected DomainEvent()
        {
            EventoId = Guid.NewGuid();
            OcurridoEn = DateTime.UtcNow;
        }
    }
}