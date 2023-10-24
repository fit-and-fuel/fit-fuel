using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
using fit_and_fuel.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fit_and_fuel.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        private readonly INutritionists _nutritionists;
        private readonly IPatients _patients;
        private readonly IDietPlan _dietPlan;
        private readonly IAppoitments _appoitments;
        private readonly IPost _post;


        public AdminController(IUserService userService, INutritionists nutritionists, IPatients patients, IDietPlan dietPlan, IAppoitments appointments, IPost post)
        {
            _userService = userService;
            _nutritionists = nutritionists;
            _patients = patients;
            _dietPlan = dietPlan;
            _appoitments = appointments;
            _post = post;
        }


        [Authorize(Roles = "Admin")]

        public async Task<IActionResult> Index()

        {
            var adminvm = new AdminVM
            {
                NumOfNutritionist = await _nutritionists.Count(),
                NumberOfAppoitment = await _appoitments.Count(),
                NumberOfPost = await _post.Count(),
                NumberOfDietPlan = await _dietPlan.Count(),
                NumOfPatient = await _patients.Count(),
            };
            return View(adminvm);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Nutritionist()
        {
            var Nutritionist = await _nutritionists.GetAll();
            return View(Nutritionist);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Posts()
        {
            var posts = await _post.GetAllPosts();
            return View(posts);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Patients()
        {
            var posts = await _patients.GetAll();
            return View(posts);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Role(string userName)
        {
            await _userService.AssignRolesToUser(userName);
            return RedirectToAction("Index", "Admin");
        }


        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> ConfiremPost(int id)
        {
            await _post.ImprovedPost(id);

            return RedirectToAction("Index", "Admin");
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]

        public async Task<IActionResult> DeletePost(int id)
        {
            await _post.Delete(id);

            return RedirectToAction("Posts", "Admin");
        }
    }
}
