using fit_and_fuel.Data;
using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;

using fit_and_fuel.Model;
using Microsoft.EntityFrameworkCore;

namespace fit_and_fuel.Services
{
    public class  ClinicService: IClinic
    {
        private readonly AppDbContext _context;

        public ClinicService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Deletes a clinic by its ID.
        /// </summary>
        /// <param name="id">The ID of the clinic to delete.</param>

        public async Task Delete(int id)
        {
            var clininc=await GetById(id);
            _context.Clinics.Remove(clininc);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves a list of all clinics.
        /// </summary>
        /// <returns>A list of all clinics.</returns>

        public async Task<List<Clinic>> GetAll()
        {
           var clinics =await _context.Clinics
                .Include(p => p.nutritionist)
                .ToListAsync();
            return clinics;
        }

        public async Task<List<ClinicDtoView>> GetAllDto()
        {
            var clinics = await GetAll();
            var clinicsToReturn = clinics.Select(clinic=>new ClinicDtoView
            {
                Id = clinic.Id,
                NutritionistId = clinic.NutritionistId,
                Name = clinic.Name,
                Address = clinic.Address,
                PhoneNumber = clinic.PhoneNumber
            }
            ).ToList();
            return clinicsToReturn;
        }

        /// <summary>
        /// Retrieves a clinic by the user ID of its associated nutritionist.
        /// </summary>
        /// <param name="UserId">The user ID of the associated nutritionist.</param>
        /// <returns>The clinic associated with the nutritionist.</returns>

        public async Task<Clinic> GetById(int UserId)
        {
            var Nut = await _context.Nutritionists.FirstOrDefaultAsync(x => x.UserId == UserId);

            int newNutId = Nut.Id;

            var clinic = await _context.Clinics.Where(c => c.NutritionistId == newNutId).FirstOrDefaultAsync();
               
            return clinic;
        }

        public async Task<ClinicDtoView> GetByIdDto(int id)
        {
        var clinic = await GetById(id);
            var clinicToReturn = new ClinicDtoView()
            {
                Id = clinic.Id,
                NutritionistId = clinic.NutritionistId,
                Name = clinic.Name,
                Address = clinic.Address,
                PhoneNumber = clinic.PhoneNumber
            };
            return clinicToReturn;
        }

        /// <summary>
        /// Creates a new clinic associated with a nutritionist.
        /// </summary>
        /// <param name="clinicDto">The details of the clinic to create.</param>
        /// <param name="UserId">The user ID of the associated nutritionist.</param>

        public async Task Post(ClinicDto clinicDto,int UserId)
        {
            var Nut = await _context.Nutritionists.FirstOrDefaultAsync(x => x.UserId == UserId);

            var clinic = new Clinic();
            clinic.Name = clinicDto.Name;   
            clinic.Address = clinicDto.Address;
            clinic.PhoneNumber = clinicDto.PhoneNumber;
            clinic.NutritionistId =Nut.Id;
            await _context.Clinics.AddAsync(clinic);
            await _context.SaveChangesAsync();  


        }

       

        /// <summary>
        /// Updates the details of a clinic associated with a nutritionist.
        /// </summary>
        /// <param name="clinicDto">The updated details of the clinic.</param>
        /// <param name="UserId">The user ID of the associated nutritionist.</param>

        public async Task Put( ClinicDto clinicDto , int UserId)
        {
            var clinic =await GetById(UserId); 
            clinic.Name = clinicDto.Name;
            clinic.Address = clinicDto.Address;
            clinic.PhoneNumber = clinicDto.PhoneNumber;
            await _context.SaveChangesAsync();


        }

    }
}
