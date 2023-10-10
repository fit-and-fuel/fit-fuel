namespace fit_and_fuel.Model
{
    public class Payment
    {
        public int Id { get; set; }
        public string PaymentId { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; } 
        public int NutritionistId { get; set; }
        public Nutritionist Nutritionist { get; set; }
        public bool IsDone { get; set; } = false;
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
