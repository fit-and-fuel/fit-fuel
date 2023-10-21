using fit_and_fuel.Model;
using System.ComponentModel.DataAnnotations;

namespace fit_and_fuel.DTOs
{
    public class PatientDto
    {

		[Required(ErrorMessage = "Name is required.")]
		public string Name { get; set; }

        public string Gender { get; set; }

		[Required(ErrorMessage = "Age is required.")]
		public int Age { get; set; }

        public int Height { get; set; }

        public int Weight { get; set; }

        public string SideEffict { get; set; }

        public string PhoneNumber { get; set; }

        public string imgURl { get; set; }

       

    }
}
