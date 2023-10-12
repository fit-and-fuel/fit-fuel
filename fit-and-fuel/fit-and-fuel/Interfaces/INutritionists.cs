using fit_and_fuel.DTOs;
using fit_and_fuel.Model;

namespace fit_and_fuel.Interfaces
{
    public interface INutritionists
    {
        Task<List<Nutritionist>> GetAll();
        Task<List<NutritionistDtoView>> GetAllDto();
        Task<List<Nutritionist>> Search(string str);
        Task<List<NutritionistDtoView>> SearchDto(string str);

        Task<Nutritionist> GetById(int id);
        Task<NutritionistDtoView> GetByIdDto(int id);
        Task<List<Patient>> GetAllMyPatient(int UserId);

        Task<List<PatientDtoViewNut>> GetAllMyPatientDto(int UserId);
        Task<List<DietPlan>> GetAllDietPlan(int UserId);
        Task<List<DietPlanDtoView>> GetAllDietPlanDto(int UserId);
        Task<Nutritionist> GetMyProfile();
        Task<NutritionistDtoView> GetMyProfileDto(int id);

        Task<Nutritionist> Post(NutritionistDto nutritionistDto);
        Task Put(int id, NutritionistDto nutritionistDto);
        Task Delete(int id);
        
    }
}
