using System.ComponentModel.DataAnnotations;

namespace fit_and_fuel.Model
{
    public class AvailableTime
    {
        public int Id { get; set; }
        public int NutritionistId { get; set; }

        public Nutritionist nutritionist { get; set; }


        [Required(ErrorMessage = "Day of week is required.")]
        public DayOfWeek DayOfWeek { get; set; }

        [Required(ErrorMessage = "Time is required.")]
        public TimeSpan Time { get; set; }
    }
}
