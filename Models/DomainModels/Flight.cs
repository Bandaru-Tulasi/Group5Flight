using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Group5Flight.Models.Validation;

namespace Group5Flight.Models.DomainModels
{
    public class Flight
    {
        public int FlightId { get; set; }

        [Required(ErrorMessage = "Flight Code is required.")]
        [RegularExpression(@"^[A-Za-z]{2}[0-9]{1,4}$", ErrorMessage = "Flight Code must start with 2 letters followed by 1-4 digits.")]
        [Remote(action: "CheckFlightCodeAndDate", controller: "Flights", areaName: "Airline", AdditionalFields = "Date", ErrorMessage = "This Flight Code and Date combination already exists.")]
        public string FlightCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "From city is required.")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "From city must contain only letters.")]
        [StringLength(50, ErrorMessage = "From city cannot exceed 50 characters.")]
        public string From { get; set; } = string.Empty;

        [Required(ErrorMessage = "To city is required.")]
        [RegularExpression(@"^[A-Za-z\s]+$", ErrorMessage = "To city must contain only letters.")]
        [StringLength(50, ErrorMessage = "To city cannot exceed 50 characters.")]
        public string To { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date is required.")]
        [DataType(DataType.Date)]
        [FutureDateWithinYears(3, ErrorMessage = "Date must be between tomorrow and 3 years from today.")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "Departure Time is required.")]
        [DataType(DataType.DateTime)]
        public DateTime DepartureTime { get; set; }

        [Required(ErrorMessage = "Arrival Time is required.")]
        [DataType(DataType.DateTime)]
        public DateTime ArrivalTime { get; set; }

        [Required(ErrorMessage = "Cabin Type is required.")]
        public string CabinType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Emission level is required.")]
        public string Emission { get; set; } = string.Empty;

        [Required(ErrorMessage = "Aircraft Type is required.")]
        public string AircraftType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required.")]
        [PriceRange(0, 50000, ErrorMessage = "Price must be between $0 and $50,000.")]
        public decimal Price { get; set; }

        public int AirlineId { get; set; }

        public Airline? Airline { get; set; }

        public ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();

        public TimeSpan Duration
        {
            get { return ArrivalTime - DepartureTime; }
        }

        public string DurationDisplay
        {
            get
            {
                var duration = Duration;
                return string.Format("{0}h {1}m", (int)duration.TotalHours, duration.Minutes);
            }
        }
    }
}