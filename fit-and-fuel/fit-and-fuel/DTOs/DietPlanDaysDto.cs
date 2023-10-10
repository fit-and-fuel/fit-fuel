namespace fit_and_fuel.DTOs
{
    public class DietPlanDaysDto
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PatientId { get; set; }

        public List<MealToPlanDto> Meals { get; set; }

        
    }
}
