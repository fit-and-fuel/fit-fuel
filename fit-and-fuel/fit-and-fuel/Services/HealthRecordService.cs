using fit_and_fuel.Data;
using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

public class HealthRecordService : IHealthRecord
{
    private readonly AppDbContext _context;
    private readonly IHttpContextAccessor _httpContextAccessor;


    public HealthRecordService(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _context = context;
        _httpContextAccessor = httpContextAccessor;
    }


    /// <summary>
    /// Deletes a health record associated with a patient's user ID.
    /// </summary>
    /// <param name="id">The user ID of the patient whose health record to delete.</param>

    public async Task Delete(string id)
    {
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == id);
        var recordtodelete = await _context.healthRecords.FirstOrDefaultAsync(p => p.PatientId == patient.Id);

        _context.healthRecords.Remove(recordtodelete);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Retrieves a list of health records associated with a nutritionist's user ID.
    /// </summary>
    /// <param name="UserId">The user ID of the nutritionist.</param>
    /// <returns>A list of health records for the nutritionist's patients.</returns>

    public async Task<List<HealthRecord>> GetAll(string UserId)
    {
        var nut = await _context.Nutritionists
            .Include(n => n.patients)


            .ThenInclude(p => p.healthRecord)
             .FirstOrDefaultAsync(n => n.UserId == UserId);

        var healthRecords = nut.patients
        .Select(p => p.healthRecord)
       .ToList();


        return healthRecords;
    }
    //public int Id { get; set; }
    //public int PatientId { get; set; }
    //public double Height { get; set; }
    //public double Weight { get; set; }
    //public double BMI { get; set; }
    //public string Illnesses { get; set; }
    public async Task<List<HealthRecordDtoView>> GetAllDto(string UserId)
    {
        var health = await GetAll(UserId);
        var healthToReturn = health.Select(h=>new HealthRecordDtoView
        {
            Id = h.Id,
            PatientId = h.PatientId,
            Height = h.Height,
            Weight = h.Weight,
            BMI = h.BMI,
            Illnesses = h.Illnesses,
        }).ToList();
        return healthToReturn;
    }
    /// <summary>
    /// Retrieves health records for a nutritionist's patients.
    /// </summary>
    /// <param name="UserId">The user ID of the nutritionist.</param>
    /// <returns>List of health records for the nutritionist's patients.</returns>

    public async Task<List<HealthRecord>> ForMyPatinets(string UserId)
    {
        var nuts = await _context.Nutritionists
                .Where(p => p.UserId == UserId)
                .Include(p => p.patients)
                .ThenInclude(p => p.appoitments)
                .ToListAsync();
        return null;
    }
    public async Task<List<HealthRecordDtoView>> ForMyPatinetsDto(string UserId)
    {
        var health = await ForMyPatinets(UserId);
        var healthToReturn = health.Select(h => new HealthRecordDtoView
        {
            Id = h.Id,
            PatientId = h.PatientId,
            Height = h.Height,
            Weight = h.Weight,
            BMI = h.BMI,
            Illnesses = h.Illnesses,
        }).ToList();
        return healthToReturn;
    }
    /// <summary>
    /// Retrieves a health record associated with a patient's user ID.
    /// </summary>
    /// <param name="id">The user ID of the patient.</param>
    /// <returns>The health record for the specified patient.</returns>

    public async Task<HealthRecord> GetMyHealthRecord(string id)
    {
        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == id);
        var record = await _context.healthRecords.Where(h => h.PatientId == patient.Id).FirstOrDefaultAsync();
        return record;
    }
    public async Task<HealthRecordDtoView> GetMyHealthRecordDto(string id)
    {
        var h = await GetMyHealthRecord(id);
        var healthToReturn =  new HealthRecordDtoView
        {
            Id = h.Id,
            PatientId = h.PatientId,
            Height = h.Height,
            Weight = h.Weight,
            BMI = h.BMI,
            Illnesses = h.Illnesses,
        };
        return healthToReturn;
    }
    /// <summary>
    /// Creates a new health record for a patient.
    /// </summary>
    /// <param name="id">The user ID of the patient.</param>
    /// <param name="HealthRecordDto">The details of the health record to create.</param>
    /// <returns>The newly created health record.</returns>

    public async Task<HealthRecord> Post( HealthRecordDto HealthRecordDto)
    {
        string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);

        var newRecord = new HealthRecord()
        {
            PatientId = patient.Id,
            Height = HealthRecordDto.Height,
            Weight = HealthRecordDto.Weight,
            Illnesses = HealthRecordDto.Illnesses

        };



        await _context.healthRecords.AddAsync(newRecord);
        await _context.SaveChangesAsync();

        return newRecord;
    }

    /// <summary>
    /// Updates a patient's health record with new details.
    /// </summary>
    /// <param name="id">The user ID of the patient.</param>
    /// <param name="healthRecordDto">The updated details of the health record.</param>

    public async Task<HealthRecord> Put(HealthRecordDto healthRecordDto)
    {
        string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var patient = await _context.Patients.FirstOrDefaultAsync(p => p.UserId == userId);
        var recordtoupdate = await _context.healthRecords.FirstOrDefaultAsync(p => p.PatientId == patient.Id);
        recordtoupdate.Height = healthRecordDto.Height;
        recordtoupdate.Weight = healthRecordDto.Weight;
        recordtoupdate.Illnesses = healthRecordDto.Illnesses;

        await _context.SaveChangesAsync();
        return recordtoupdate;

    }

    public Task<List<HealthRecord>> GetAll(int UserId)
    {
        throw new NotImplementedException();
    }

    public Task<List<HealthRecordDtoView>> GetAllDto(int UserId)
    {
        throw new NotImplementedException();
    }

    public Task<HealthRecord> GetMyHealthRecord(int id)
    {
        throw new NotImplementedException();
    }

    public Task<HealthRecordDtoView> GetMyHealthRecordDto(int id)
    {
        throw new NotImplementedException();
    }

    public Task<List<HealthRecord>> ForMyPatinets(int UserId)
    {
        throw new NotImplementedException();
    }

    public Task<List<HealthRecordDtoView>> ForMyPatinetsDto(int UserId)
    {
        throw new NotImplementedException();
    }

  

    public Task Put(int UserId, HealthRecordDto patientDto)
    {
        throw new NotImplementedException();
    }

    public Task Delete(int id)
    {
        throw new NotImplementedException();
    }
}

