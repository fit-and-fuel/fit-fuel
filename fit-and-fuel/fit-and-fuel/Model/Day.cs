using fit_and_fuel.Interfaces;

namespace fit_and_fuel.Model
{
    public class Day
    {
        public int Id { get; set; }

        public string DayName { get; set; }

        public DateTime Date { get; set; }

        public int TotalCalories { get { return meals.Sum(meal => meal.Calories); } }
        public int DietPlanId { get; set; }
        public DietPlan dietPlan { get; set; }
        public ICollection<Meal> meals { get; set; }
    }
}
