namespace fit_and_fuel.Model
{
    public class Clinic
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }

        public int NutritionistId { get; set; }

        public Nutritionist nutritionist { get; set; }
    }
}
