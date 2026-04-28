using System.ComponentModel.DataAnnotations;

namespace Group5Flight.Models.DomainModels
{
    public class Reservation
    {
        public int ReservationId { get; set; }

        [Required]
        public int FlightId { get; set; }

        public Flight Flight { get; set; } = null!;

        public DateTime ReservedDate { get; set; } = DateTime.Now;
    }
}