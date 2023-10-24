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
        private readonly IPost _post;
        private readonly IClinic _clinic;
        private readonly IComment _comment;
        private readonly IPrice _price;



        public NutritionistController(INutritionists nutritionists,
            IPrice price,
            IAvailableTime availableTime, IAppoitments appoitments, IDietPlan dietplan, IMeals meals, IPost post, IClinic clinic, IComment comment)
        {
            _nutritionists = nutritionists;
            _availableTime = availableTime;
            _appoitments = appoitments;
            _dietplan = dietplan;
            _meals = meals;
            _post = post;
            _clinic = clinic;
            _comment = comment;
            _price = price;

        }

        public async Task<IActionResult> ViewAllNutrition(string searchTerm)
        {
            var nut = await _nutritionists.GetAllDto();

            if (!string.IsNullOrEmpty(searchTerm))
            {
                searchTerm = searchTerm.Trim();
                nut = nut
                    .Where(p => p.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();
            }
            return View(nut);


        }

        [Authorize(Roles = "Nutritionist")]
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

        [Authorize(Roles = "Nutritionist")]
        public IActionResult CreateProfile()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> CreateProfile(NutritionistDto nut, IFormFile file, IFormFile cvfile)
        {
            //if (!ModelState.IsValid)
            //{
            //	return View();
            //}
            ModelState.Remove("file");
            ModelState.Remove("cvfile");


            var nutri = await _nutritionists.Post(nut, file, cvfile);

            return RedirectToAction("NutDetails", new { id = nutri.Id });
        }


        [Authorize(Roles = "Nutritionist")]
        public async Task<IActionResult> MyProfile()
        {
            var av = await _nutritionists.GetMyProfile();
            return View(av);
        }

        [Authorize(Roles = "Nutritionist")]
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

        [Authorize(Roles = "Nutritionist")]
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



        [Authorize(Roles = "Patient")]
        [HttpPost]
        public async Task<IActionResult> SelectAppoitment(int id)
        {
            await _appoitments.SelectAppoitment(id);
            return RedirectToAction("Index", "Home");
        }


        [Authorize(Roles = "Nutritionist")]
        [HttpPost]
        public async Task<IActionResult> AppoitmentConfirmed(int id)
        {
            await _appoitments.AppoitmentConfirmed(id);
            return Redirect("Appointments");
        }

        [Authorize(Roles = "Nutritionist")]
        public async Task<IActionResult> CreateDietPlan(int id)
        {
            var diet = new DietPlanDto();
            diet.PatientId = id;
            return View(diet);
        }


        [Authorize(Roles = "Nutritionist")]

        [HttpPost]

        public async Task<IActionResult> CreateDietPlan(DietPlanDto diteplain)
        {
            var d = await _dietplan.PostDietPlanWithDay(diteplain);

            return RedirectToAction("MyPatientDietPlan", new { id = d.Id });


        }

        [Authorize(Roles = "Nutritionist")]
        [HttpPost]
        public async Task<IActionResult> AddMeal(MealDto meal)
        {
            await _meals.Post(meal);
            return Redirect($"MyPatientDietPlan/{meal.DietPlanId}");

        }

        [Authorize(Roles = "Nutritionist")]
        [HttpPost]
        public async Task<IActionResult> EditMeal(int id, MealDto meal)
        {

            await _meals.Put(id, meal);
            return RedirectToAction("MyPatientDietPlan", new { id = meal.DietPlanId });

        }


        [Authorize(Roles = "Nutritionist")]
        public IActionResult AddPost()
        {
            return View();
        }


        [Authorize(Roles = "Nutritionist")]
        [HttpPost]
        public async Task<IActionResult> AddPost(PostDto post, IFormFile file)
        {
            ModelState.Remove("file");

            //if (!ModelState.IsValid)
            //{
            //    return View();
            //}

            await _post.Post(post, file);

            return RedirectToAction("Index", "Home");

        }


        [Authorize(Roles = "Nutritionist")]
        public async Task<IActionResult> ViewPosts()
        {
            var posts = await _post.GetMyPosts();
            return View(posts);
        }


        [Authorize(Roles = "Nutritionist")]
        public IActionResult AddClinic()
        {
            return View();
        }

        [Authorize(Roles = "Nutritionist")]

        [HttpPost]
        public async Task<IActionResult> AddClinic(ClinicDto clinicDto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var clinic = await _clinic.Post(clinicDto);
            // redirect to profile nutrition
            return Redirect($"NutDetails/{clinic.NutritionistId}");

        }

        [Authorize(Roles = "Nutritionist")]
        public IActionResult AddPrice()
        {
            return View();
        }

        [Authorize(Roles = "Nutritionist")]
        [HttpPost]
        public async Task<IActionResult> AddPrice(PriceDto priceDto)
        {
            var pri = await _price.Post(priceDto);
            // redirect to profile nutrition
            return Redirect($"NutDetails/{pri.NutritionistId}");

        }

        [Authorize(Roles = "Nutritionist")]
        public async Task<IActionResult> CompleteAppointment(int id)
        {
            await _appoitments.AppoitmentCompleted(id);
            return RedirectToAction("Appointments", "Nutritionist");

        }





    }
}



