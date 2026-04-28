using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using Group5Flight.Models.DomainModels;

namespace Group5Flight.Models.ViewModels
{
    public class FlightEditViewModel
    {
        public Flight Flight { get; set; } = new Flight();

        public List<SelectListItem> AirlineList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> CabinTypeList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> AircraftTypeList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> EmissionList { get; set; } = new List<SelectListItem>();
    }
}