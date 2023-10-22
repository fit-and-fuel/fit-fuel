namespace fit_and_fuel.Model
{
    public class Price
    {
        public int Id { get; set; }
        public double amount { get; set; }
        public int NutritionistId { get; set; }
        public Nutritionist Nutritionist { get; set; }
    }
}
