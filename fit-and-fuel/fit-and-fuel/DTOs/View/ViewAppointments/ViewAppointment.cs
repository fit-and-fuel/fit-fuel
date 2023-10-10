using fit_and_fuel.Model;

namespace fit_and_fuel.DTOs.View.ViewAppointments
{
    public class ViewAppointment
    {
        public int Id { get; set; }
        public string Status { get; set; }

        public DateTime Time { get; set; }

        public bool? IsConfirmed { get; set; } = false;

        public bool? IsCompleted { get; set; } = false;
        public PatientView Patient { get; set; }
        public NutritionView nutritionist { get; set; }

    }
}
