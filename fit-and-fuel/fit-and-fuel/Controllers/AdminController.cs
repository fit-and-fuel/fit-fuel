using Microsoft.AspNetCore.Mvc;

namespace fit_and_fuel.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index()
        {
            return View();
        }
    }
}
