namespace fit_and_fuel.DTOs
{
    public class HealthRecordDtoView
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public double BMI { get; set; }
        public string Illnesses { get; set; }
    }
}
