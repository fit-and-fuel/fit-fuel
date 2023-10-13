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
        private readonly IAppoitments _appoitments;
        private readonly IDietPlan _dietplan;
		private readonly IMeals _meals;

        public NutritionistController(INutritionists nutritionists, IAvailableTime availableTime, IAppoitments appoitments, IDietPlan dietplan, IMeals meals)
		{
			_nutritionists = nutritionists;
			_availableTime = availableTime;
			_appoitments = appoitments;
             _dietplan = dietplan;
			_meals = meals;

        }
		public async Task<IActionResult> Appointments()
		{
			var Appoitment = await _appoitments.GetMyById();
			return View(Appoitment);
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

        public async Task<IActionResult> MyProfile()
        {
            var av = await _nutritionists.GetMyProfile();
            return View(av);
        }

        public async Task<IActionResult> MyPatientDietPlan(int id)
        {
            var dietplan = await _dietplan.GetById(id);
            return View(dietplan);
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
		[HttpPost]
        public async Task<IActionResult> SelectAppoitment(int id)
        {
			await _appoitments.SelectAppoitment(id);
            return RedirectToAction("Index","Home");
        }
        [HttpPost]
        public async Task<IActionResult> AppoitmentConfirmed(int id)
        {
            await _appoitments.AppoitmentConfirmed(id);
            return Redirect("Index");
		}


        public async Task<IActionResult> CreateDietPlan(int id)
		{
			var dite = new DietPlanDto(); 
			dite.PatientId= id;
            return View(dite);
		}

        [HttpPost]

		public async Task<IActionResult> CreateDietPlan(DietPlanDto diteplain)
		{
			 await _dietplan.PostDietPlanWithDay(diteplain);

			return Redirect("MyProfile");

		}

     
        [HttpPost]
        public async Task<IActionResult> AddMeal(MealDto meal)
        {
            await _meals.Post(meal);

			return Redirect($"MyProfile");

        }




    }
}

	

	