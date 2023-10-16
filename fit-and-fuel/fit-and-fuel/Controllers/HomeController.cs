using fit_and_fuel.Interfaces;
using fit_and_fuel.Models;
using fit_and_fuel.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace fit_and_fuel.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly INutritionists _nutritionists;
        private readonly IPost _post;

        public HomeController(ILogger<HomeController> logger, INutritionists nutritionists ,IPost post )
        {
            _logger = logger;
            _nutritionists = nutritionists;
            _post = post;
        }

        public async Task<IActionResult> Index()
        {
            var post =await _post.GetAll();

            var nutritionist = await _nutritionists.GetAll();
            var HomeView=new HomeViewModel();
            HomeView.nutritionists=nutritionist;
            HomeView.posts=post;

            return View(HomeView);

        }

        public async Task<IActionResult> Post(int id)
        {
            var post = await _post.GetById(id);

            return View(post);

        }

        public async Task<IActionResult> AllPost()
        {
            var post = await _post.GetAll();

            return View(post);

        }


        public IActionResult About()
        {
            return View();
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