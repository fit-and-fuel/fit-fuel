using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace fit_and_fuel.Controllers
{
    public class ClientController : Controller
    {
		private readonly IPatients _patients;
        private readonly IRating _rating;
        public ClientController(IPatients patients,IRating rating)
        {
            _rating = rating;
            _patients = patients;
        }
		[Authorize(Roles = "Patient")]
		public IActionResult Index()
        {
            return View();
        }
        public async Task<IActionResult> MyProfile()
        {
            var myprofile = await _patients.GetMyProfile();
            return View(myprofile);
        }
        [Authorize(Roles = "Patient")]
		[HttpPost]
        public async Task<IActionResult> Create(PatientDto patientDto, IFormFile file)
        {
            ModelState.Remove("file");


            await _patients.Post(patientDto, file);
            return RedirectToAction("Index", "Home");
        }
		public async Task<IActionResult> MyDietPlan()
		{
            var dietplan = await _patients.GetMyDietPlan();
			return View(dietplan);
		}
        public async Task<IActionResult> MealForToday()
        {
            var meals = await _patients.GetMyMealsForToday();
            return View(meals);
        }
        [HttpPost]
        public async Task<IActionResult> Completion(int id)
        {
            await _patients.MealIsCompletion(id);
            return RedirectToAction("MealForToday");
        }
        public ActionResult AddRating()
        {
           
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddRating(RatingDto ratingDto)
        {
            await _rating.AddRating(ratingDto);
            return RedirectToAction("index");
        }
    }
}
