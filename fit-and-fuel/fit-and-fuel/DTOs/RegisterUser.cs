using System.ComponentModel.DataAnnotations;

namespace fit_and_fuel.DTOs
{
    public class RegisterUser
    {
        //[Required]
        //public int UserId { get; set; }
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string Email { get; set; }



        public string PhoneNumber { get; set; }

        public List<string> Roles { get; set; }
    }
}
