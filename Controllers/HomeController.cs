using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Group5Flight.Models.DataLayer;
using Group5Flight.Models.DomainModels;
using Group5Flight.Models.Utilities;
using Group5Flight.Models.ViewModels;

namespace Group5Flight.Controllers
{
    public class HomeController : Controller
    {
        private readonly Repository<Flight> _flightRepo;
        private readonly Repository<Airline> _airlineRepo;
        private readonly Repository<Reservation> _reservationRepo;

        public HomeController(AirBnBContext context)
        {
            _flightRepo = new Repository<Flight>(context);
            _airlineRepo = new Repository<Airline>(context);
            _reservationRepo = new Repository<Reservation>(context);
        }

        public IActionResult Index()
        {
            var session = new FlightSession(HttpContext.Session);

            if (session.GetSelectedFlightCount() == 0)
            {
                var cookies = new FlightCookie(Request.Cookies);
                var cookieIds = cookies.GetSelectedFlightIds();
                if (cookieIds.Any())
                {
                    session.SetSelectedFlights(cookieIds);
                }
            }

            var flights = _flightRepo.List(new QueryOptions<Flight>
            {
                Includes = "Airline"
            }).AsQueryable();

            var from = session.GetFrom();
            var to = session.GetTo();
            var cabin = session.GetCabinType();
            var dateStr = session.GetDate();
            var airlineId = session.GetAirlineId();

            if (!string.IsNullOrEmpty(from))
            {
                flights = flights.Where(f => f.From == from);
            }
            if (!string.IsNullOrEmpty(to))
            {
                flights = flights.Where(f => f.To == to);
            }
            if (!string.IsNullOrEmpty(cabin) && cabin != "All")
            {
                flights = flights.Where(f => f.CabinType == cabin);
            }
            if (!string.IsNullOrEmpty(dateStr))
            {
                if (DateTime.TryParse(dateStr, out var date))
                {
                    flights = flights.Where(f => f.Date.Date == date.Date);
                }
            }
            if (airlineId.HasValue)
            {
                flights = flights.Where(f => f.AirlineId == airlineId.Value);
            }

            var vm = new HomeViewModel
            {
                Flights = flights.ToList(),
                From = from,
                To = to,
                CabinType = cabin,
                DepartureDate = string.IsNullOrEmpty(dateStr) ? DateTime.Now.AddDays(1) : DateTime.Parse(dateStr),
                Airlines = _airlineRepo.List(new QueryOptions<Airline>()).ToList(),
                AirlineId = airlineId,
                FromCities = _flightRepo.List(new QueryOptions<Flight>()).Select(f => f.From).Distinct().ToList(),
                ToCities = _flightRepo.List(new QueryOptions<Flight>()).Select(f => f.To).Distinct().ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Filter(HomeViewModel vm)
        {
            var session = new FlightSession(HttpContext.Session);
            session.SetFrom(vm.From);
            session.SetTo(vm.To);
            session.SetCabinType(vm.CabinType);
            session.SetAirlineId(vm.AirlineId);
            if (vm.DepartureDate.HasValue)
            {
                session.SetDate(vm.DepartureDate.Value.ToString("yyyy-MM-dd"));
            }
            return RedirectToAction("Index");
        }

        public IActionResult Search()
        {
            return Content("Search flights page - routing test for Phase 1");
        }

        public IActionResult Privacy()
        {
            return Content("Privacy policy page - routing test for Phase 1");
        }

        public IActionResult Details(int id)
        {
            var flight = _flightRepo.Get(id);
            if (flight == null)
            {
                return NotFound();
            }
            var session = new FlightSession(HttpContext.Session);
            ViewBag.From = session.GetFrom();
            ViewBag.To = session.GetTo();
            ViewBag.Date = session.GetDate();
            ViewBag.CabinType = session.GetCabinType();
            ViewBag.AirlineId = session.GetAirlineId();
            return View(flight);
        }

        [HttpPost]
        public IActionResult Select(int id)
        {
            var session = new FlightSession(HttpContext.Session);
            session.AddSelectedFlight(id);
            var cookies = new FlightCookie(Response.Cookies);
            cookies.SetSelectedFlightIds(session.GetSelectedFlights());
            var flight = _flightRepo.Get(id);
            var flightName = flight != null ? $"{flight.From} to {flight.To}" : "Flight";
            TempData["Message"] = $"{flightName} selected successfully";
            return RedirectToAction("Index");
        }

        public IActionResult Selection()
        {
            var session = new FlightSession(HttpContext.Session);
            var selectedIds = session.GetSelectedFlights();
            var flights = _flightRepo.List(new QueryOptions<Flight>
            {
                Includes = "Airline"
            }).Where(f => selectedIds.Contains(f.FlightId)).ToList();
            return View(flights);
        }

        [HttpPost]
        public IActionResult Remove(int id)
        {
            var session = new FlightSession(HttpContext.Session);
            session.RemoveSelectedFlight(id);
            var cookies = new FlightCookie(Response.Cookies);
            cookies.SetSelectedFlightIds(session.GetSelectedFlights());
            TempData["Message"] = "Flight removed from selection";
            return RedirectToAction("Selection");
        }

        [HttpPost]
        public IActionResult ClearAll()
        {
            var session = new FlightSession(HttpContext.Session);
            session.ClearSelectedFlights();
            var cookies = new FlightCookie(Response.Cookies);
            cookies.RemoveSelectedFlightIds();
            TempData["Message"] = "All selections cleared";
            return RedirectToAction("Selection");
        }

        [HttpPost]
        public IActionResult Reserve()
        {
            var session = new FlightSession(HttpContext.Session);
            var selectedIds = session.GetSelectedFlights();
            if (!selectedIds.Any())
            {
                TempData["Error"] = "No flights selected to reserve.";
                return RedirectToAction("Selection");
            }
            var existingReservations = _reservationRepo.List(new QueryOptions<Reservation>())
                .Where(r => selectedIds.Contains(r.FlightId))
                .Select(r => r.FlightId)
                .ToList();
            var newReservations = selectedIds
                .Where(id => !existingReservations.Contains(id))
                .Select(id => new Reservation { FlightId = id, ReservedDate = DateTime.Now })
                .ToList();
            foreach (var reservation in newReservations)
            {
                _reservationRepo.Insert(reservation);
            }
            _reservationRepo.Save();
            TempData["Message"] = $"{newReservations.Count} flights reserved successfully.";
            return RedirectToAction("Selection");
        }
    }
}