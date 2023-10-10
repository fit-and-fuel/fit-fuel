using fit_and_fuel.Model;

namespace fit_and_fuel.DTOs
{
    public class PatientDtoView
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public int? NutritionistId { get; set; }
        public string Name { get; set; }

        public string Gender { get; set; }

        public int Age { get; set; }


        public string PhoneNumber { get; set; }

        public string imgURl { get; set; }
    
        
       
    }
}
