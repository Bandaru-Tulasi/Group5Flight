using Microsoft.AspNetCore.Mvc;

namespace Group5Flight.Controllers
{
    public class SearchController : Controller
    {
        public IActionResult Index()
        {
            return Content("Search flights page - routing test for Phase 1");
        }
    }
}