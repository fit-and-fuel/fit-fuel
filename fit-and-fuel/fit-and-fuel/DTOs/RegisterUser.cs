using System.ComponentModel.DataAnnotations;

namespace fit_and_fuel.DTOs
{
    public class RegisterUser
    {
		//[Required]
		//public int UserId { get; set; }
		[Required(ErrorMessage = "UserName is required.")]
		public string Username { get; set; }

		[Required(ErrorMessage = "Password is required.")]
		public string Password { get; set; }

		[Required(ErrorMessage = "Email is required.")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Phone number is required.")]
		public string PhoneNumber { get; set; }

        public List<string> Roles { get; set; }
    }
}
