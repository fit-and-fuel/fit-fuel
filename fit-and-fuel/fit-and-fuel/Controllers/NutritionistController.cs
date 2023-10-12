using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fit_and_fuel.Controllers
{

	public class NutritionistController : Controller
	{
		private readonly INutritionists _nutritionists;
		private readonly IAvailableTime _availableTime;
		public NutritionistController(INutritionists nutritionists, IAvailableTime availableTime)
		{
			_nutritionists = nutritionists;
			_availableTime = availableTime;
		}
		public IActionResult Index()
		{
			return View();
		}

		public async Task<IActionResult> NutDetails(int id)
		{
			var nut = await _nutritionists.GetById(id);
			return View(nut);
		}
		public IActionResult CreateProfile()
		{
			return View();
		}


		[HttpPost]
        public async Task<IActionResult> CreateProfile(NutritionistDto nut)
        {
			//if (!ModelState.IsValid)
			//{
			//	return View();
			//}
			await _nutritionists.Post(nut);

			return RedirectToAction("Index","Home");
        }

        [Authorize(Roles = "Nutritionist")]

        public async Task<IActionResult> AvailableTimes()
		{
			var av = await _availableTime.GetAll();
			return View(av);
		}
		public async Task<IActionResult> AddAvailableTime()
		{

			return View();
		}

		[HttpPost]
		public async Task<IActionResult> AddAvailableTime(AvailableTimeDto availableTimeDto)
		{

			await _availableTime.Post(availableTimeDto);
			return Redirect("AvailableTimes");
		}
	}
}

	

	