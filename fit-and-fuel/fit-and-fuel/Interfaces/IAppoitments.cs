using fit_and_fuel.DTOs;
using fit_and_fuel.DTOs.View.ViewAppointments;
using fit_and_fuel.Model;

namespace fit_and_fuel.Interfaces
{
    public interface IAppoitments
    {
        Task<List<ViewAppointment>> GetAll();
        Task<ViewAppointment> GetById(int id);
        Task<List<ViewAppointment>> GetMyById();
        Task<List<ViewAppointment>> GetMyByIdForPatient(int id);
        Task<Appoitment> Post(string UserId,AppoitmentDto appoitmentDto,int NutritionistId);
        Task AppoitmentConfirmed(int id);
        Task<Appoitment> SelectAppoitment(int timeId);
        Task<bool> AppoitmentCompleted(string UserId, int id);
        Task Put(int id, AppoitmentDto appoitmentDto);
        Task Delete(int id);
		Task<int> Count();
	}
}
