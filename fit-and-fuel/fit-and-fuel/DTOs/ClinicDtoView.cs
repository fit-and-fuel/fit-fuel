using fit_and_fuel.Model;

namespace fit_and_fuel.DTOs
{
    public class ClinicDtoView
    {
        public int Id { get; set; }
        public int NutritionistId { get; set; }
        public string Name { get; set; }

        public string Address { get; set; }

        public string PhoneNumber { get; set; }
    }
}
