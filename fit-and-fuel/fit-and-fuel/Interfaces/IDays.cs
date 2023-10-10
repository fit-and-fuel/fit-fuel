using fit_and_fuel.DTOs;
using fit_and_fuel.Model;

namespace fit_and_fuel.Interfaces
{
    public interface IDays
    {
        Task<List<Day>> GetAll();

        Task<List<DayDtoView>> GetAllDto();
        Task<Day> GetById(int id);
        Task<DayDtoView> GetByIdDto(int id);
        Task<Day> Post(DayDto day);
        Task<DayDtoView> PostDto(DayDto day);
        Task Put(int id, DayDto day);
        Task Delete(int id);
    }
}
