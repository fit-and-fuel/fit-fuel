using fit_and_fuel.Model;

namespace fit_and_fuel.DTOs
{
    public class MealDtoView
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int DayId { get; set; }
        public string Description { get; set; }
        public int Calories { get; set; }

        public bool Completion { get; set; } 
        
       
    }
}
