namespace fit_and_fuel.Model
{
    public class Appoitment
    {
        public int Id { get; set; }
        public string Status { get; set; }

        private DateTime _time;

        public DateTime Time
        {
            get => _time;
            set => _time = value > DateTime.Now ? value : throw new ArgumentException("Appointment time must be in the future.");
        }
       
       
        public bool? IsConfirmed { get; set; }=false;

        public bool? IsCompleted { get; set; } = false;
        
        public int PatientId { get; set; }
        public int NutritionistId { get; set; }
        public Patient Patient { get; set; }
        public Nutritionist nutritionist { get; set; }
    }
}
