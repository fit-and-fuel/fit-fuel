namespace fit_and_fuel.Model
{
    public class HealthRecord
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public double BMI
        {
            get
            {


                // Calculate BMI auto
                if (Height > 0)
                {
                    return Weight / ((Height / 100.0) * (Height / 100.0));
                }
                else
                {
                    return 0;

                }


            }


        }
        public string Illnesses { get; set; }

        public Patient Patient { get; set; }
    }
}
