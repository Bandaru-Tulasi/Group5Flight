using Group5Flight.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Group5Flight.Controllers
{
    public class HomeController : Controller
    {
        private readonly AirBnBContext _context;

        public HomeController(AirBnBContext context)
        {
            _context = context;
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

            var flights = _context.Flights
                .Include(f => f.Airline)
                .AsQueryable();

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
                Airlines = _context.Airlines.ToList(),
                AirlineId = airlineId,
                FromCities = _context.Flights.Select(f => f.From).Distinct().ToList(),
                ToCities = _context.Flights.Select(f => f.To).Distinct().ToList()
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
            var flight = _context.Flights
                .Include(f => f.Airline)
                .FirstOrDefault(f => f.FlightId == id);

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

            var flight = _context.Flights.Find(id);
            var flightName = flight != null ? $"{flight.From} to {flight.To}" : "Flight";
            TempData["Message"] = $"{flightName} selected successfully";

            return RedirectToAction("Index");
        }

        public IActionResult Selection()
        {
            var session = new FlightSession(HttpContext.Session);
            var selectedIds = session.GetSelectedFlights();

            var flights = _context.Flights
                .Include(f => f.Airline)
                .Where(f => selectedIds.Contains(f.FlightId))
                .ToList();

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
    }
}