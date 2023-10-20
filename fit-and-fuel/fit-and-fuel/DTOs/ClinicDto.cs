using fit_and_fuel.Model;
using System.ComponentModel.DataAnnotations;

namespace fit_and_fuel.DTOs
{
    public class ClinicDto
    {
		[Required(ErrorMessage = "Name is required.")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Address is required.")]
		public string Address { get; set; }

		[Required(ErrorMessage = "PhoneNumber is required.")]
		public string PhoneNumber { get; set; }


        
    }
}
