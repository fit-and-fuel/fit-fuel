using System.ComponentModel.DataAnnotations;

namespace fit_and_fuel.DTOs
{
    public class LoginData
    {
        [Required(ErrorMessage = "UserName is required.")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; }
    }
}
