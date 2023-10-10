using System.ComponentModel.DataAnnotations;

namespace fit_and_fuel.DTOs
{
    public class LoginData
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}
