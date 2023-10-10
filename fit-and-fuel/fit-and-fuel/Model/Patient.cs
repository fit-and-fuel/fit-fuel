namespace fit_and_fuel.Model
{
    public class Patient
    {
        public int Id { get; set; }

        public string? UserId { get; set; }
        public string Name { get; set; }

        public string Gender { get; set; }

        public int Age { get; set; }


        public string PhoneNumber { get; set; }

        public HealthRecord healthRecord { get; set; }

        public string imgURl { get; set; }

        public ICollection<Appoitment> appoitments { get; set; }
        public int? NutritionistId { get; set; }
        public Nutritionist? nutritionist { get; set; }

        public DietPlan dietPlan { get; set; }

        

    }
}
