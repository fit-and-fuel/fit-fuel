using fit_and_fuel.DTOs;
using fit_and_fuel.Model;

namespace fit_and_fuel.Interfaces
{
    public interface IClinic
    {
        Task<List<Clinic>> GetAll();
        Task<List<ClinicDtoView>> GetAllDto();
        Task<Clinic> GetById(int id);
        Task<ClinicDtoView> GetByIdDto(int id);
        Task Post(ClinicDto clinicDto,int UserId);
        Task Post(ClinicDto clinicDto);


        Task Put(ClinicDto clinicDto,int UserId);
        Task Delete(int Userid);
    }
}
