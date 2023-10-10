using fit_and_fuel.DTOs;
using fit_and_fuel.DTOs.View.ViewAvilableTme;
using fit_and_fuel.Model;

namespace fit_and_fuel.Interfaces
{
    public interface IAvailableTime
    {
        Task<List<AvailableTime>> GetAll(int NutritionistId);

        Task<List<ViewAvilableTime>> GetAllDto(int NutritionistId);

        Task Post(int UserId, AvailableTimeDto availableTimeDto);

        Task Put(int UserId, AvailableTimeDto availableTimeDto, int id);
        Task Delete(int UserId,int id);
    }
}
