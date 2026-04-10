using System;

namespace Group5Flight.Models
{
    public class Flight
    {
        public int FlightId { get; set; }

        public string FlightCode { get; set; } = string.Empty;

        public string From { get; set; } = string.Empty;
        public string To { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public string CabinType { get; set; } = string.Empty;

        public string Emission { get; set; } = string.Empty;

        public string AircraftType { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int AirlineId { get; set; }

        public Airline? Airline { get; set; }

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