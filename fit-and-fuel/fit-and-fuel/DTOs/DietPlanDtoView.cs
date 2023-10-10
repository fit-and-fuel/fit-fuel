using fit_and_fuel.Model;

namespace fit_and_fuel.DTOs
{
    public class DietPlanDtoView
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int NutritionistId { get; set; }
        public int Duration { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<DayDtoView> days { get; set; }
    
    }
}
