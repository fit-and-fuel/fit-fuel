using System.ComponentModel.DataAnnotations;

namespace fit_and_fuel.DTOs
{
    public class HealthRecordDto
    {
        [Required(ErrorMessage = "Height is required.")]
        public double Height { get; set; }
        [Required(ErrorMessage = "Weight is required.")]
        public double Weight { get; set; }
        public string Illnesses { get; set; }

    }
}
