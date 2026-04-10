using Microsoft.AspNetCore.Mvc;

namespace Group5Flight.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Manage()
        {
            return Content("Admin managing users - routing test for Phase 1");
        }

        public IActionResult Rights()
        {
            return Content("Admin rights and obligations - routing test for Phase 1");
        }
    }
}