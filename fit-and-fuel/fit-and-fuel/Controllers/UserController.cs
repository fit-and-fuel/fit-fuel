using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace fit_and_fuel.Controllers
{
    public class UserController : Controller
    {
        private readonly IEmailSender _emailSender;
        private IUserService _userService;


        public UserController(IUserService userService, IEmailSender emailSender)
        {
            _userService = userService;
            _emailSender = emailSender;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<UserDto>> Register(RegisterUser data)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            bool IsNutritionist = false;
            if (data.Roles[0] == "Nutritionist")
            {
                IsNutritionist = true;

            }
            var res = await _userService.Register(data, this.ModelState);
            if (IsNutritionist)
            {
                // this for Send Email
                //await _emailSender.EmailToUser(data.Email, data.Username);
            }
            var resRole = await _userService.Authenticate(data.Username, data.Password, this.ModelState);

            if (!ModelState.IsValid)
            {
                return View(res);
            }
            if (resRole.Roles.Count == 0)
            {
                return RedirectToAction("Index", "Home");

            }

            if (resRole.Roles[0] == "Patient")
            {
                return RedirectToAction("Index", "Client");
            }

            return RedirectToAction("Index", "Home");
        }
        public IActionResult Login()
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            return View();
        }
        [HttpPost]
        public async Task<ActionResult<UserDto>> Login(LoginData data)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var res = await _userService.Authenticate(data.Username, data.Password, this.ModelState);
            if (!ModelState.IsValid)
            {
                return View(res);
            }
            if (res == null)
            {
				return RedirectToAction("Login", "User");

			}
			if (res.Roles[0] == "Admin")
            {
                return RedirectToAction("Index", "Admin");
            }

            return RedirectToAction("Index", "Home");
        }
        public async Task<IActionResult> Logout()
        {
            await _userService.Logout();
            return Redirect("/");
        }
    }


    ////[Authorize(Roles = "Patient,Admin,Nutritionist")]
    //[HttpGet("me")]
    //public async Task<ActionResult<UserDto>> Me()
    //{
    //    // Following the [Authorize] phase, this.User will be ... you.
    //    // Put a breakpoint here and inspect to see what's passed to our getUser method
    //    return await _userService.GetUser(this.User);
    //}

    //[Authorize(Roles = "Patient,Admin,Nutritionist")]
    //[HttpPost]
    //[Route("change-password")]
    //public async Task<ActionResult> ChangePassword(PasswordChangeModel model)
    //{
    //    if (!ModelState.IsValid)
    //    {
    //        return BadRequest(ModelState);
    //    }

    //    var userId = User.Identity.Name;
    //    var success = await _userService.ChangePasswordAsync(userId, model.CurrentPassword, model.NewPassword);

    //    if (success)
    //    {
    //        return Ok("Password changed successfully.");
    //    }
    //    else
    //    {
    //        return BadRequest("Failed to change password.");
    //    }
    //}
    //[HttpPut("assign-roles/{UserId}")]
    //[Authorize(Roles = "Admin")]
    //public async Task<IActionResult> AssignRolesToUser(int UserId)
    //{
    //    var result = await _userService.AssignRolesToUser(UserId);

    //    if (result.Succeeded)
    //    {
    //        return Ok();
    //    }

    //    return BadRequest(result.Errors);
    //}

}

