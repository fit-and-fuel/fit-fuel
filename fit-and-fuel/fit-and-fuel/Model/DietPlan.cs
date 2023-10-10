namespace fit_and_fuel.Model
{
    public class DietPlan
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public int NutritionistId { get; set; }
        public int Duration { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public ICollection<Day> days { get; set; }
        
        public Patient Patient { get; set; }

        
        public Nutritionist nutritionist { get; set; }


    }
}
