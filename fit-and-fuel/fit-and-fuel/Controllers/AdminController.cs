using fit_and_fuel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace fit_and_fuel.Controllers
{
    public class AdminController : Controller
    {
        private readonly IUserService _userService;

        public AdminController(IUserService userService)
        {
            _userService = userService;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Role(string userName)
        {
            await _userService.AssignRolesToUser(userName);
            return RedirectToAction("Index","Home");
        }
    }
}
