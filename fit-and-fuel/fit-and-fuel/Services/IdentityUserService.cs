using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;

using fit_and_fuel.Model;
//using Humanizer;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
//using NuGet.Configuration;
using System.Data;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;

namespace fit_and_fuel.Services
{
    public class IdentityUserService : IUserService
    {
        private UserManager<ApplicationUser> userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private INotification _notificationService;
        private readonly IEmailSender _emailSender;
        //private JwtTokenService jwtTokenService;
        public IdentityUserService(UserManager<ApplicationUser> manager,
            //JwtTokenService jwtTokenService, 
            SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration, INotification notificationService, IEmailSender emailSender)
        {
            userManager = manager;
            _signInManager = signInManager;
            //this.jwtTokenService = jwtTokenService
            _notificationService = notificationService;
            _emailSender = emailSender;
        }

        /// <summary>
        /// Authenticates a user by validating the provided username and password.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>UserDto containing user information and authentication token if successful, null if authentication fails.</returns>

        public async Task<UserDto> Authenticate(string username, string password, ModelStateDictionary model)
        {
            var result = await _signInManager.PasswordSignInAsync(username, password, true, false);
            var users = await userManager.FindByNameAsync(username);

            if (result.Succeeded)
            {
                var user = await userManager.FindByNameAsync(username);
                //// user = await _userManager.FindByNameAsync(username);
                return new UserDto()
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Roles = await userManager.GetRolesAsync(user)
                };
            }
            if (users == null)
            {
                model.AddModelError("Username", "User name is not correct");
            }
            else
            {
                model.AddModelError("Password", "Password is not correct");
            }
            return null;

        }

        /// <summary>
        /// Registers a new user based on provided data and assigns roles.
        /// </summary>
        /// <param name="data">The registration data of the new user.</param>
        /// <param name="modelState">The ModelStateDictionary to add errors if registration fails.</param>
        /// <returns>UserDto containing user information and authentication token if successful, null if registration fails.</returns>

        public async Task<UserDto> Register(RegisterUser data, ModelStateDictionary modelState)
        {
            var user = new ApplicationUser
            {
                UserName = data.Username,
                Email = data.Email,
                PhoneNumber = data.PhoneNumber
            };

            var result = await userManager.CreateAsync(user, data.Password);

            if (result.Succeeded)
            {
                if (data.Roles[0] == "Patient")
                {
                    await userManager.AddToRolesAsync(user, data.Roles);
                }
                else if (data.Roles[0] == "Nutritionist")
                {

                    var content = new NotificationDto()
                    {
                        Content = $"We have New Nutritionist Need To Be Approved with UserName {user.UserName}"
                    };

                    await _notificationService.SendNotification("1", content);

                }

                var userDTO = new UserDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    //Token = await jwtTokenService.GetToken(user, System.TimeSpan.FromDays(1)),
                    Roles = await userManager.GetRolesAsync(user)

                };

                return userDTO;
            }

            foreach (var error in result.Errors)
            {
                var errorKey =
                  error.Code.Contains("Password") ? nameof(data.Password) :
                  error.Code.Contains("Email") ? nameof(data.Email) :
                  error.Code.Contains("UserName") ? nameof(data.Username) :
                  "";

                modelState.AddModelError(errorKey, error.Description);
            }

            return null;

        }

        /// <summary>
        /// Reads the last used user ID from a JSON file or returns a default value if reading fails.
        /// </summary>
        /// <returns>The last used user ID or 0 if file reading fails.</returns>

        //private int ReadLastUsedUserId()
        //{
        //    try
        //    {
        //        var json = File.ReadAllText(_lastUsedUserIdFilePath);
        //        var jsonObject = JObject.Parse(json);
        //        return jsonObject.GetValue("LastUsedUserId").Value<int>();
        //    }
        //    catch
        //    {
        //        return 0; // Default value if file reading fails
        //    }
        //}

        /// <summary>
        /// Updates the last used user ID in a JSON file.
        /// </summary>
        /// <param name="newUserId">The new user ID to be updated.</param>

        //private void UpdateLastUsedUserId(int newUserId)
        //{
        //    var jsonObject = new JObject();
        //    jsonObject["LastUsedUserId"] = newUserId;

        //    File.WriteAllText(_lastUsedUserIdFilePath, jsonObject.ToString());
        //}

        /// <summary>
        /// Retrieves user information based on the provided ClaimsPrincipal.
        /// </summary>
        /// <param name="principal">ClaimsPrincipal representing the user's claims.</param>
        /// <returns>UserDto containing user information and authentication token.</returns>

        public async Task<UserDto> GetUser(ClaimsPrincipal principal)
        {
            var user = await userManager.GetUserAsync(principal);
            return new UserDto
            {
                Id = user.Id,
                Username = user.UserName,
                //Token = await jwtTokenService.GetToken(user, System.TimeSpan.FromDays(1)),
                Roles = await userManager.GetRolesAsync(user)
            };
        }

        /// <summary>
        /// Changes the password of a user.
        /// </summary>
        /// <param name="userId">The ID of the user whose password to change.</param>
        /// <param name="currentPassword">The user's current password.</param>
        /// <param name="newPassword">The new password to set.</param>
        /// <returns>True if the password change was successful, false otherwise.</returns>

        public async Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await userManager.FindByNameAsync(userId);

            if (user == null)
            {
                return false;
            }

            var changePasswordResult = await userManager.ChangePasswordAsync(user, currentPassword, newPassword);

            return changePasswordResult.Succeeded;
        }



        /// <summary>
        /// Assigns the "Nutritionist" role to a user.
        /// </summary>
        /// <param name="userId">The ID of the user to assign the role to.</param>
        /// <returns>IdentityResult indicating the success or failure of the role assignment.</returns>

        public async Task<IdentityResult> AssignRolesToUser(string userName)
        {
            var user = await userManager.FindByNameAsync(userName);
            if (user == null)
            {

                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }


            await userManager.AddToRoleAsync(user, "Nutritionist");
            // this for email 
            await _emailSender.EmailToUserRole(user.Email, userName);
            var content = new NotificationDto()
            {
                Content = $"Welcome {userName}, you are now a Nutritionist.\r\nYou can now create your profile by clicking on this" +
                $" https://localhost:7035/nutritionist/CreateProfile "
            };

            await _notificationService.SendNotification(user.Id, content);

            return IdentityResult.Success;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }




    }
}


