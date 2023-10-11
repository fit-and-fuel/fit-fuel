using fit_and_fuel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace fit_and_fuel.Controllers
{

	public class NutritionistController : Controller
	{
		private readonly INutritionists _nutritionists;

		public NutritionistController(INutritionists nutritionists)
		{
			_nutritionists = nutritionists;
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
	}
}

	

	