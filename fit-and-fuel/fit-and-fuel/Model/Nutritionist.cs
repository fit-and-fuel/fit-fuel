using fit_and_fuel.Interfaces;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.ComponentModel.DataAnnotations;

namespace fit_and_fuel.Model
{
    public class Nutritionist
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? UserId { get; set; }
        public string Gender { get; set; }

        public int Age { get; set; }

        public string PhoneNumber { get; set; }

        public string CvURl { get; set; }

		public string imgURl { get; set; }

        public double? AverageRating
        {
            get
            {
                if (Ratings == null || Ratings.Count == 0)
                {
                    return null;
                }
                return Ratings.Where(r => r != null && r.Value != null).Average(r => r.Value);
            }
        }


        public List<AvailableTime>? AvaliableTimes { get; set; }
        public Clinic? clinic { get; set; }
        public ICollection<Appoitment> appoitments { get; set; }
        public ICollection<Patient> patients { get; set; }

        public ICollection<DietPlan> dietPlans { get; set; }

        public ICollection<Post> posts { get; set; }

        public ICollection<Rating> Ratings { get; set; }



    }
}
