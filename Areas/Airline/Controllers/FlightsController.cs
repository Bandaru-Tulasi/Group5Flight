using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Group5Flight.Models;

namespace Group5Flight.Areas.Airline.Controllers
{
    [Area("Airline")]
    public class FlightsController : Controller
    {
        private readonly AirBnBContext _context;

        public FlightsController(AirBnBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var flights = _context.Flights
                .Include(f => f.Airline)
                .OrderBy(f => f.Date)
                .ThenBy(f => f.DepartureTime)
                .ToList();

            return View(flights);
        }

        public IActionResult Manage()
        {
            return Content("Manage flights page - routing test for Phase 1");
        }

        public IActionResult Regulation()
        {
            return Content("Airline regulations page - routing test for Phase 1");
        }

        [HttpGet]
        public IActionResult Add()
        {
            var vm = CreateFlightEditViewModel(new Flight());
            return View("AddEdit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(FlightEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                _context.Flights.Add(vm.Flight);
                _context.SaveChanges();

                TempData["Message"] = $"Flight {vm.Flight.FlightCode} added successfully";
                return RedirectToAction("Index");
            }

            vm = CreateFlightEditViewModel(vm.Flight);
            return View("AddEdit", vm);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var flight = _context.Flights.Find(id);

            if (flight == null)
            {
                return NotFound();
            }

            var vm = CreateFlightEditViewModel(flight);
            return View("AddEdit", vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(FlightEditViewModel vm)
        {
            if (ModelState.IsValid)
            {
                var existingFlight = _context.Flights.Find(vm.Flight.FlightId);

                if (existingFlight == null)
                {
                    return NotFound();
                }

                existingFlight.FlightCode = vm.Flight.FlightCode;
                existingFlight.From = vm.Flight.From;
                existingFlight.To = vm.Flight.To;
                existingFlight.Date = vm.Flight.Date;
                existingFlight.DepartureTime = vm.Flight.DepartureTime;
                existingFlight.ArrivalTime = vm.Flight.ArrivalTime;
                existingFlight.CabinType = vm.Flight.CabinType;
                existingFlight.Emission = vm.Flight.Emission;
                existingFlight.AircraftType = vm.Flight.AircraftType;
                existingFlight.Price = vm.Flight.Price;
                existingFlight.AirlineId = vm.Flight.AirlineId;

                _context.SaveChanges();

                TempData["Message"] = $"Flight {vm.Flight.FlightCode} updated successfully";
                return RedirectToAction("Index");
            }

            vm = CreateFlightEditViewModel(vm.Flight);
            return View("AddEdit", vm);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var flight = _context.Flights
                .Include(f => f.Airline)
                .FirstOrDefault(f => f.FlightId == id);

            if (flight == null)
            {
                return NotFound();
            }

            return View(flight);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int FlightId)
        {
            var flight = _context.Flights.Find(FlightId);

            if (flight == null)
            {
                return NotFound();
            }

            var flightCode = flight.FlightCode;
            _context.Flights.Remove(flight);
            _context.SaveChanges();

            TempData["Message"] = $"Flight {flightCode} deleted successfully";
            return RedirectToAction("Index");
        }

        private FlightEditViewModel CreateFlightEditViewModel(Flight flight)
        {
            var vm = new FlightEditViewModel
            {
                Flight = flight
            };

            vm.AirlineList = _context.Airlines
                .OrderBy(a => a.Name)
                .Select(a => new SelectListItem
                {
                    Value = a.AirlineId.ToString(),
                    Text = a.Name
                }).ToList();

            vm.CabinTypeList = HomeViewModel.CabinTypes
                .Where(c => c != "All")
                .Select(c => new SelectListItem
                {
                    Value = c,
                    Text = c
                }).ToList();

            vm.AircraftTypeList = HomeViewModel.AircraftTypes
                .Select(a => new SelectListItem
                {
                    Value = a,
                    Text = a
                }).ToList();

            vm.EmissionList = new List<SelectListItem>
            {
                new SelectListItem { Value = "Low", Text = "Low" },
                new SelectListItem { Value = "Medium", Text = "Medium" },
                new SelectListItem { Value = "High", Text = "High" }
            };

            return vm;
        }
    }
}