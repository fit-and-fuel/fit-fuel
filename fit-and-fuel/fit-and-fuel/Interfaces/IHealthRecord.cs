using fit_and_fuel.DTOs;
using fit_and_fuel.Model;

namespace fit_and_fuel.Interfaces
{
    public interface IHealthRecord
    {
        Task<List<HealthRecord>> GetAll(int UserId);
        Task<List<HealthRecordDtoView>> GetAllDto(int UserId);
        Task<HealthRecord> GetMyHealthRecord(int id);
        Task<HealthRecordDtoView> GetMyHealthRecordDto(int id);
        Task<List<HealthRecord>> ForMyPatinets(int UserId);
        Task<List<HealthRecordDtoView>> ForMyPatinetsDto(int UserId);
        Task<HealthRecord> Post(int UserId, HealthRecordDto HealthRecordDto);

        Task Put(int UserId, HealthRecordDto patientDto);
        Task Delete(int id);
    }
}
