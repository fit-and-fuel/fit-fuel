namespace fit_and_fuel.Model
{
    public class Rating
    {
        public int Id { get; set; }
        public int NutritionistId { get; set; }
        public int PatientId { get; set; }
        public int Value { get; set; }


        public string Comment { get; set; }
    }
}
