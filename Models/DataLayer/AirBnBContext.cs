using Microsoft.EntityFrameworkCore;
using Group5Flight.Models.DomainModels;

namespace Group5Flight.Models.DataLayer
{
    public class AirBnBContext : DbContext
    {
        public AirBnBContext(DbContextOptions<AirBnBContext> options)
            : base(options)
        {
        }

        public DbSet<Flight> Flights { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AirBnBContext).Assembly);
        }
    }
}