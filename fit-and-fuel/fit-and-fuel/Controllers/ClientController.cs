using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace fit_and_fuel.Controllers
{
    public class ClientController : Controller
    {
        private readonly IPatients _patients;
        private readonly IRating _rating;
        private readonly IHealthRecord _healthRecord; 
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _config;


        public ClientController(IPatients patients, IRating rating, IHealthRecord healthRecord, IConfiguration config)
        {
            _rating = rating;
            _patients = patients;
            _healthRecord = healthRecord;
            _config = config;   
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
        public async Task<IActionResult> DetailPatient(int Id)
        {
            var Patient = await _patients.GetById(Id);
            return View(Patient);
        }

        [Authorize(Roles = "Patient")]
        [HttpPost]
        public async Task<IActionResult> Create(PatientDto patientDto, IFormFile file)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View("Index");
            //}
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
            // redirect to nutrition profile
            return RedirectToAction("index","Home");
        }



        public async Task<IActionResult> ViewHealthRecord(string Id)
        {
            var MyHealthRecord = await _healthRecord.GetMyHealthRecord(Id);
            return View(MyHealthRecord);
        }

        public ActionResult AddHealthRecord()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> AddHealthRecord(HealthRecordDto health)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var Health = await _healthRecord.Post(health);
            //redirect ot profile patient
            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Payment()
        {
            var patient = await _patients.GetMyProfile();

            StripeConfiguration.ApiKey = _config.GetSection("SettingStrip:SecretKey").Get<string>();

            var domain = "https://localhost:7035/";

            var options = new SessionCreateOptions
            {
                SuccessUrl = domain + "Home",
                CancelUrl = domain,
                LineItems = new List<SessionLineItemOptions>(),
                Mode = "payment",
            };


            var sessionLineItem = new SessionLineItemOptions
            {
                PriceData = new SessionLineItemPriceDataOptions()
                {
                    UnitAmount = (long)(patient.nutritionist.Price.amount * 100), // 20.50 => 2050
                    Currency = "usd",
                    ProductData = new SessionLineItemPriceDataProductDataOptions()
                    {
                        Name = patient.nutritionist.Name
                    }
                },
                Quantity = 1
                };

                options.LineItems.Add(sessionLineItem);
            

            var service = new SessionService();
            var session = service.Create(options);

            var sessionId = session.Id;

            TempData["sessionId"] = sessionId;


            Response.Headers.Add("Location", session.Url);

            return new StatusCodeResult(303);
        }
    }
}
