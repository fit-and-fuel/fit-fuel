namespace fit_and_fuel.Model
{
    public class Meal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Calories { get; set; }

        public bool Completion { get; set; }=false;
        public int DayId { get; set; }
        public Day day { get; set; }
        
    }
}
