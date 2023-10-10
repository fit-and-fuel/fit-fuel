namespace fit_and_fuel.DTOs
{
    public class MealToPlanDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int Calories { get; set; }
        public int DayNumber { get; set; }
        public bool Completion { get; set; }=false;
       
    }
}
