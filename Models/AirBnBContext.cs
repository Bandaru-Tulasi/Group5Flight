using Microsoft.EntityFrameworkCore;

namespace Group5Flight.Models
{
    public class AirBnBContext : DbContext
    {
        public AirBnBContext(DbContextOptions<AirBnBContext> options)
            : base(options)
        {
        }

        public DbSet<Flight> Flights { get; set; }
         public DbSet<Airline> Airlines { get; set; }
    }
}