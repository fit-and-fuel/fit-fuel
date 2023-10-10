using fit_and_fuel.DTOs;
using fit_and_fuel.DTOs.View.ViewAppointments;
using fit_and_fuel.Model;

namespace fit_and_fuel.Interfaces
{
    public interface IAppoitments
    {
        Task<List<ViewAppointment>> GetAll();
        Task<ViewAppointment> GetById(int id);
        Task<List<ViewAppointment>> GetMyById(int id);
        Task<List<ViewAppointment>> GetMyByIdForPatient(int id);
        Task<Appoitment> Post(int UserId,AppoitmentDto appoitmentDto,int NutritionistId);
        Task AppoitmentConfirmed(int UserId,int id);
        Task<Appoitment> SelectAppoitment(int UserId, AppoitmentWithTimeDto appoitmentDto);
        Task<bool> AppoitmentCompleted(int UserId, int id);
        Task Put(int id, AppoitmentDto appoitmentDto);
        Task Delete(int id);
    }
}
