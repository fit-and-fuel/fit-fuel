﻿using fit_and_fuel.DTOs;
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
        private readonly IConfiguration _configuration;
        private UserManager<ApplicationUser> userManager;
        private INotification _notificationService;
        private readonly string _lastUsedUserIdFilePath;
      
        private JwtTokenService jwtTokenService;
        public IdentityUserService(UserManager<ApplicationUser> manager, JwtTokenService jwtTokenService, IConfiguration configuration,INotification notificationService)
        {
            userManager = manager;
            this.jwtTokenService = jwtTokenService;
            _configuration = configuration;
            _lastUsedUserIdFilePath = "lastUsedUserId.json";
         
            _notificationService = notificationService;
        }

        /// <summary>
        /// Authenticates a user by validating the provided username and password.
        /// </summary>
        /// <param name="username">The username of the user.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>UserDto containing user information and authentication token if successful, null if authentication fails.</returns>

        public async Task<UserDto> Authenticate(string username, string password)
        {
            var user = await userManager.FindByNameAsync(username);

            if (await userManager.CheckPasswordAsync(user, password))
            {
                return new UserDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Token = await jwtTokenService.GetToken(user, System.TimeSpan.FromDays(1)),
                    Roles = await userManager.GetRolesAsync(user)

                };
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
            int lastUsedUserId = ReadLastUsedUserId();

            int newUserId = lastUsedUserId + 1;
            UpdateLastUsedUserId(newUserId);
            var user = new ApplicationUser
            {
                Id = newUserId.ToString(),
                UserName = data.Username,
                Email = data.Email,
                PhoneNumber = data.PhoneNumber
            };

            var result = await userManager.CreateAsync(user, data.Password);

            if (result.Succeeded)
            {
                if (data.Roles[0] == "Patient") { 
                await userManager.AddToRolesAsync(user, data.Roles);
                }
                else if(data.Roles[0] == "Nutritionist")
                {

                    var content = new NotificationDto()
                    {
                        Content = $"We have New Nutritionist Need To Improve with Id {user.Id}"
                    };

                    await _notificationService.SendNotification("1", content);
                   
                }

                var userDTO = new UserDto
                {
                    Id = user.Id,
                    Username = user.UserName,
                    Token = await jwtTokenService.GetToken(user, System.TimeSpan.FromDays(1)),
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

        private int ReadLastUsedUserId()
        {
            try
            {
                var json = File.ReadAllText(_lastUsedUserIdFilePath);
                var jsonObject = JObject.Parse(json);
                return jsonObject.GetValue("LastUsedUserId").Value<int>();
            }
            catch
            {
                return 0; // Default value if file reading fails
            }
        }

        /// <summary>
        /// Updates the last used user ID in a JSON file.
        /// </summary>
        /// <param name="newUserId">The new user ID to be updated.</param>

        private void UpdateLastUsedUserId(int newUserId)
        {
            var jsonObject = new JObject();
            jsonObject["LastUsedUserId"] = newUserId;

            File.WriteAllText(_lastUsedUserIdFilePath, jsonObject.ToString());
        }

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
                Token = await jwtTokenService.GetToken(user, System.TimeSpan.FromDays(1)),
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

        public async Task<IdentityResult> AssignRolesToUser(int userId)
         {
            var user = await userManager.FindByIdAsync(userId.ToString());
            if (user == null)
            {
                
                return IdentityResult.Failed(new IdentityError { Description = "User not found." });
            }

            
            await userManager.AddToRoleAsync(user, "Nutritionist");
          
            var content = new NotificationDto()
            {
                Content = $"welcome New Nutritionist"
            };

            await _notificationService.SendNotification(userId.ToString(), content);

            return IdentityResult.Success;
         }






    }

}
