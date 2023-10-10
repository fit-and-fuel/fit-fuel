using fit_and_fuel.DTOs;
using fit_and_fuel.Model;

namespace fit_and_fuel.Interfaces
{
    public interface IMeals
    {
        Task<List<Meal>> GetAll();
        Task<List<MealDtoView>> GetAllDto();
        Task<Meal> GetById(int id);
        Task<MealDtoView> GetByIdDto(int id);
        Task<Meal> Post(MealDto meal);
        Task<MealDtoView> PostDto(MealDto meal);

        Task Put(int id, MealDto meal);
        Task Delete(int id);
    }
}
