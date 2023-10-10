using fit_and_fuel.DTOs;
using fit_and_fuel.Model;

namespace fit_and_fuel.Interfaces
{
    public interface IDietPlan
    {
        Task<List<DietPlan>> GetAll();
        Task<List<DietPlanDtoView>> GetAllDto();
        Task<DietPlan> GetById(int id);
        Task<DietPlanDtoView> GetByIdDto(int id);
        Task<DietPlan> Post(DietPlanDto dietPlan);
        Task<DietPlanDtoView> PostDto(DietPlanDto dietPlan);
        Task PostFullDietPlan(DietPlanDaysDto dietPlanDays,int UserId);
        Task Put(int id, DietPlanDto dietPlan);
        Task Delete(int id);
    }
}
