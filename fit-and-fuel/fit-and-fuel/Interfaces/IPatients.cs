using fit_and_fuel.DTOs;
using fit_and_fuel.Model;

namespace fit_and_fuel.Interfaces
{
    public interface IPatients
    {
        Task<List<Patient>> GetAll();
        Task<List<PatientDtoView>> GetAllDto();
        Task<Patient> GetById(int id);
        Task<PatientDtoView> GetByIdDto(int id);

        Task SelectNut(int NutId, int UserId);
        Task<List<Patient>> Search(string str);
        Task<List<PatientDtoView>> SearchDto(string str);
        Task<DietPlan> GetMyDietPlan(int UserId);
        Task<DietPlanDtoView> GetMyDietPlanDto(int UserId);
        Task<List<Meal>> GetMyMealsForToday(int UserId);
       

        Task MealIsCompletion(int UserId, int MealId);
        Task<Patient> Post(int resId, PatientDto patientDto);
        
        Task Put(int id, PatientDto patientDto);
        Task Delete(int id);

    }
}
