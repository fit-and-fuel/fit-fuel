using fit_and_fuel.Model;

namespace fit_and_fuel.DTOs
{
    public class DietPlanDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PatientId { get; set; }

        public int NutritionistId { get; set; }
    }
}
