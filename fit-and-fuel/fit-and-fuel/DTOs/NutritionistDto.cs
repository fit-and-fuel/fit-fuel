using fit_and_fuel.Model;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace fit_and_fuel.DTOs
{
    public class NutritionistDto
    {
		[Required(ErrorMessage = "Name is required.")]
		public string Name { get; set; }

        public string Gender { get; set; }

		[Required(ErrorMessage = "Age is required.")]
		public int Age { get; set; }

        public string PhoneNumber { get; set; }

        public string CvURl { get; set; }
        public string imgURl { get; set; }


    }
}
