using fit_and_fuel.Model;

namespace fit_and_fuel.DTOs
{
    public class DayDtoView
    {
        public int Id { get; set; }

        public string DayName { get; set; }

        public DateTime Date { get; set; }

        public int TotalCalories { get; set; }
        public int DietPlanId { get; set; }
        public ICollection<MealDtoView> meals { get; set; }
    }
}
