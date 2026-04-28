using System;
using System.Collections.Generic;
using Group5Flight.Models.DomainModels;

namespace Group5Flight.Models.ViewModels
{
    public class HomeViewModel
    {
        public List<Flight> Flights { get; set; } = new List<Flight>();

        public string From { get; set; } = "";
        public string To { get; set; } = "";
        public DateTime? DepartureDate { get; set; }
        public string CabinType { get; set; } = "All";

        public int? AirlineId { get; set; }
        public List<Airline> Airlines { get; set; } = new List<Airline>();

        public List<string> FromCities { get; set; } = new List<string>();
        public List<string> ToCities { get; set; } = new List<string>();

        public static List<string> CabinTypes { get; } = new List<string>
        {
            "All",
            "Basic Economy",
            "Economy",
            "Economy Plus",
            "Business"
        };

        public static List<string> AircraftTypes { get; } = new List<string>
        {
            "Airbus 320 Family",
            "Boeing 737 Family"
        };

        public string CheckActiveCabin(string cabin)
        {
            return (cabin == CabinType) ? "active" : "";
        }
    }
}