using fit_and_fuel.Interfaces;
using fit_and_fuel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace fit_and_fuel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INutritionists _nutritionists;

        public HomeController(ILogger<HomeController> logger, INutritionists nutritionists)
        {
            _logger = logger;
            _nutritionists = nutritionists;
        }

        public async Task<IActionResult> Index()
        {
            var nutritionist = await _nutritionists.GetAll();
            return View(nutritionist);
        }


        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}