namespace fit_and_fuel.Model
{
    public class AvailableTime
    {
        public int Id { get; set; }
        public int NutritionistId { get; set; }

        public Nutritionist nutritionist { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public TimeSpan Time { get; set; }
    }
}
