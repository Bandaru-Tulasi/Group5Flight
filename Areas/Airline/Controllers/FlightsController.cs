using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Group5Flight.Models.DataLayer;
using Group5Flight.Models.DomainModels;
using Group5Flight.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Group5Flight.Areas.Airline.Controllers
{
    [Area("Airline")]
    public class FlightsController : Controller
    {
        private readonly Repository<Flight> _flightRepo;
        private readonly Repository<Models.DomainModels.Airline> _airlineRepo;
        private readonly Repository<Reservation> _reservationRepo;

        public FlightsController(AirBnBContext context)
        {
            _flightRepo = new Repository<Flight>(context);
            _airlineRepo = new Repository<Models.DomainModels.Airline>(context);
            _reservationRepo = new Repository<Reservation>(context);
        }

        public IActionResult Index()
        {
            var flights = _flightRepo.List(new QueryOptions<Flight>
            {
                Includes = "Airline"
            }).OrderBy(f => f.Date).ThenBy(f => f.DepartureTime).ToList();
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
            ValidateUniqueFlightCodeDate(vm.Flight);
            if (ModelState.IsValid)
            {
                _flightRepo.Insert(vm.Flight);
                _flightRepo.Save();
                TempData["Message"] = $"Flight {vm.Flight.FlightCode} added successfully";
                return RedirectToAction("Index");
            }
            vm = CreateFlightEditViewModel(vm.Flight);
            return View("AddEdit", vm);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var flight = _flightRepo.Get(id);
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
            ValidateUniqueFlightCodeDate(vm.Flight);
            if (ModelState.IsValid)
            {
                var existingFlight = _flightRepo.Get(vm.Flight.FlightId);
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
                _flightRepo.Update(existingFlight);
                _flightRepo.Save();
                TempData["Message"] = $"Flight {vm.Flight.FlightCode} updated successfully";
                return RedirectToAction("Index");
            }
            vm = CreateFlightEditViewModel(vm.Flight);
            return View("AddEdit", vm);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var flight = _flightRepo.Get(id);
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
            var hasReservations = _reservationRepo.List(new QueryOptions<Reservation>())
                .Any(r => r.FlightId == FlightId);
            if (hasReservations)
            {
                TempData["Error"] = "Cannot delete this flight because it has been reserved.";
                return RedirectToAction("Index");
            }
            var flight = _flightRepo.Get(FlightId);
            if (flight == null)
            {
                return NotFound();
            }
            var flightCode = flight.FlightCode;
            _flightRepo.Delete(flight);
            _flightRepo.Save();
            TempData["Message"] = $"Flight {flightCode} deleted successfully";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult CheckFlightCodeAndDate(string FlightCode, DateTime Date)
        {
            bool exists = _flightRepo.List(new QueryOptions<Flight>())
                .Any(f => f.FlightCode == FlightCode && f.Date.Date == Date.Date);
            if (!exists)
            {
                TempData["ValidatedFlightCodeDate"] = $"{FlightCode}|{Date:yyyy-MM-dd}";
            }
            return Json(!exists);
        }

        private void ValidateUniqueFlightCodeDate(Flight flight)
        {
            string currentKey = $"{flight.FlightCode}|{flight.Date:yyyy-MM-dd}";
            string cachedKey = TempData["ValidatedFlightCodeDate"] as string;
            if (cachedKey == currentKey)
            {
                return;
            }
            bool exists = _flightRepo.List(new QueryOptions<Flight>())
                .Any(f => f.FlightCode == flight.FlightCode && f.Date.Date == flight.Date.Date && f.FlightId != flight.FlightId);
            if (exists)
            {
                ModelState.AddModelError("Flight.FlightCode", "This Flight Code and Date combination already exists.");
            }
            TempData["ValidatedFlightCodeDate"] = currentKey;
        }

        private FlightEditViewModel CreateFlightEditViewModel(Flight flight)
        {
            var vm = new FlightEditViewModel
            {
                Flight = flight
            };
            vm.AirlineList = _airlineRepo.List(new QueryOptions<Models.DomainModels.Airline>())
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