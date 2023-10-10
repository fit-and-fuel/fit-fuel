using fit_and_fuel.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Security.Claims;

namespace fit_and_fuel.Interfaces
{
    public interface IUserService
    {
        public Task<UserDto> Register(RegisterUser data, ModelStateDictionary modelState);

        public Task<UserDto> Authenticate(string username, string password);

        public Task<UserDto> GetUser(ClaimsPrincipal principal);
        public  Task<bool> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
        Task Logout();
        Task<IdentityResult> AssignRolesToUser(int userId);
    }
}
