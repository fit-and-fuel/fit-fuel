using fit_and_fuel.Model;

namespace fit_and_fuel.DTOs
{
    public class MealDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Calories { get; set; }

        public bool Completion { get; set; }
        public int DayId { get; set; }
    }
}
