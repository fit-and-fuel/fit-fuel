using fit_and_fuel.Data;
using fit_and_fuel.DTOs;
using fit_and_fuel.DTOs.View.ViewAppointments;
using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
using Microsoft.EntityFrameworkCore;

//using NuGet.Protocol.Plugins;
using System;
using System.Security.Claims;

namespace fit_and_fuel.Services
{
    public class AppoitmentService : IAppoitments
    {
        private readonly AppDbContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        private INotification _notificationService;



        public AppoitmentService(AppDbContext context, INotification notificationService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;


            _notificationService = notificationService;
        }
        /// <summary>
        /// Deletes an appointment by its ID.
        /// </summary>
        /// <param name="id">The ID of the appointment to delete.</param>
        public async Task Delete(int id)
        {
            var appoitment = await _context.Appoitments.Where(a => a.Id == id).FirstOrDefaultAsync();
            _context.Appoitments.Remove(appoitment);
            await _context.SaveChangesAsync();
        }


        /// <summary>
        /// Retrieves a list of all appointments.
        /// </summary>
        /// <returns>A list of all appointments.</returns>

        public async Task<List<ViewAppointment>> GetAll()

        {
            var appoitments = await _context.Appoitments.Include(x => x.Patient).Include(d => d.nutritionist).Select(a => new ViewAppointment
            {
                Id = a.Id,
                IsCompleted = a.IsCompleted,
                IsConfirmed = a.IsConfirmed,
                Time = a.Time,
                Status = a.Status,
                Patient = new PatientView
                {
                    Id = a.Id,
                    Name = a.Patient.Name,
                    Gender = a.Patient.Gender,


                },
                nutritionist = new NutritionView
                {
                    Id = a.Id,
                    Name = a.nutritionist.Name,
                    Gender = a.nutritionist.Gender
                }

            })
             .ToListAsync();
            return appoitments;
        }


        /// <summary>
        /// Confirms an appointment for a specific user and appointment ID.
        /// </summary>
        /// <param name="UserId">The ID of the user confirming the appointment.</param>
        /// <param name="id">The ID of the appointment to confirm.</param>


        public async Task AppoitmentConfirmed(int id)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var appoitments = await _context.Appoitments
                .Where(a => a.nutritionist.UserId == userId).ToListAsync();
            var appoitment = appoitments.Where(a => a.Id == id).FirstOrDefault();
            if (appoitment != null)
            {
                appoitment.IsConfirmed = true;
                var patient = await _context.Patients
                    .Include(p => p.appoitments)
                    .SingleOrDefaultAsync(x => x.Id == appoitment.PatientId);

                patient.NutritionistId = appoitment.NutritionistId;

                var content = new NotificationDto()
                {
                    Content = "Your  Appoitment is Confirmed "
                };

                await _notificationService.SendNotification(patient.UserId.ToString(), content);

                await _context.SaveChangesAsync();
            }

            var time = await _context.AvailableTime
                .Where(a => a.NutritionistId == appoitment.NutritionistId)
                .Where(t => t.Time == appoitment.Time.TimeOfDay).FirstOrDefaultAsync();
            if (time != null)
            {
                _context.AvailableTime.Remove(time);
                await _context.SaveChangesAsync();
            }


        }

        /// <summary>
        /// Marks an appointment as completed for a specific user and appointment ID.
        /// </summary>
        /// <param name="UserId">The ID of the user marking the appointment as completed.</param>
        /// <param name="id">The ID of the appointment to mark as completed.</param>
        /// <returns>True if the appointment was marked as completed, otherwise false.</returns>

        public async Task<bool> AppoitmentCompleted(int id)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;


            var appoitments = await _context.Appoitments
                .Where(a => a.nutritionist.UserId == userId).ToListAsync();

            var appoitment = appoitments.Where(a => a.Id == id).FirstOrDefault();

            var patient = await _context.Patients.Where(p => p.Id == appoitment.PatientId).FirstOrDefaultAsync();

            patient.NutritionistId = null;
            var dietplan = await _context.DietPlans.Where(d => d.PatientId == patient.Id).FirstOrDefaultAsync();
            if (dietplan != null)
            {
                _context.DietPlans.Remove(dietplan);
                await _context.SaveChangesAsync();
            }
            if (appoitment.IsConfirmed == true)
            {
                appoitment.IsCompleted = true;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }


        public async Task<ViewAppointment> GetById(int id)
        {
            var appoitments = await _context.Appoitments.Where(x => x.Id == id).Include(x => x.Patient).Include(d => d.nutritionist).Select(a => new ViewAppointment
            {
                Id = a.Id,
                IsCompleted = a.IsCompleted,
                IsConfirmed = a.IsConfirmed,
                Status = a.Status,
                Time = a.Time,
                Patient = new PatientView
                {
                    Id = a.Id,
                    Name = a.Patient.Name,
                    Gender = a.Patient.Gender,


                },
                nutritionist = new NutritionView
                {
                    Id = a.Id,
                    Name = a.nutritionist.Name,
                    Gender = a.nutritionist.Gender
                }


            }).FirstOrDefaultAsync();

            return appoitments;
        }


        /// <summary>
        /// Retrieves a list of appointments associated with a specific user ID.
        /// </summary>
        /// <param name="id">The ID of the user whose appointments are to be retrieved.</param>
        /// <returns>A list of appointments associated with the user.</returns>

        public async Task<List<ViewAppointment>> GetMyById()

        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var appoitments = await _context.Appoitments.Where(a => a.nutritionist.UserId == userId).Include(x => x.Patient).Include(d => d.nutritionist).Select(a => new ViewAppointment
            {
                Id = a.Id,
                Time = a.Time,
                IsCompleted = a.IsCompleted,
                IsConfirmed = a.IsConfirmed,
                Status = a.Status,
                Patient = new PatientView
                {
                    Id = a.Patient.Id,

                    Name = a.Patient.Name,
                    Gender = a.Patient.Gender,


                },
                nutritionist = new NutritionView
                {
                    Id = a.nutritionist.Id,

                    Name = a.nutritionist.Name,
                    Gender = a.nutritionist.Gender
                }
            }
          ).ToListAsync();
            return appoitments;
        }


        /// <summary>
        /// Retrieves a list of appointments associated with a specific patient user ID.
        /// </summary>
        /// <param name="id">The ID of the patient user whose appointments are to be retrieved.</param>
        /// <returns>A list of appointments associated with the patient user.</returns>

        public async Task<List<ViewAppointment>> GetMyByIdForPatient(string id)

        {
            var appoitments = await _context.Appoitments
                .Where(a => a.Patient.UserId == id).Include(x => x.Patient).Include(d => d.nutritionist).Select(a => new ViewAppointment
                {
                    Id = a.Id,
                    Time = a.Time,
                    IsCompleted = a.IsCompleted,
                    IsConfirmed = a.IsConfirmed,
                    Status = a.Status,
                    Patient = new PatientView
                    {
                        Id = a.Id,

                        Name = a.Patient.Name,
                        Gender = a.Patient.Gender
                    },
                    nutritionist = new NutritionView
                    {
                        Id = a.Id,
                        Name = a.nutritionist.Name,
                        Gender = a.nutritionist.Gender
                    }
                }
  ).ToListAsync();

            return appoitments;
        }

        /// <summary>
        /// Creates a new appointment for a patient with the specified details.
        /// </summary>
        /// <param name="UserId">The ID of the patient user creating the appointment.</param>
        /// <param name="appoitmentDto">The details of the appointment to create.</param>
        /// <param name="NutritionistId">The ID of the nutritionist associated with the appointment.</param>
        /// <returns>The newly created appointment.</returns>

        public async Task<Appoitment> Post(string UserId, AppoitmentDto appoitmentDto, int NutritionistId)
        {
            var patinet = await _context.Patients
                .Include(p => p.nutritionist)
                .Where(p => p.UserId == UserId).FirstOrDefaultAsync();

            var appoitmentToAdd = new Appoitment();
            appoitmentToAdd.Time = appoitmentDto.Time;
            appoitmentToAdd.Status = appoitmentDto.Status;
            appoitmentToAdd.PatientId = patinet.Id;
            if (patinet.NutritionistId == null)
            {
                appoitmentToAdd.NutritionistId = NutritionistId;
            }
            else if (patinet.NutritionistId == NutritionistId)
            {
                appoitmentToAdd.NutritionistId = NutritionistId;
            }
            else
            {
                throw new Exception($"you have Nutritionist {patinet.nutritionist.Name}");
            }
            await _context.Appoitments.AddAsync(appoitmentToAdd);
            await _context.SaveChangesAsync();

            return appoitmentToAdd;
            ;


        }

        /// <summary>
        /// Selects an appointment slot for a patient with the specified details.
        /// </summary>
        /// <param name="UserId">The ID of the patient user selecting the appointment slot.</param>
        /// <param name="appoitmentDto">The details of the selected appointment slot.</param>
        /// <returns>The newly selected appointment.</returns>

        public async Task<Appoitment> SelectAppoitment(int timeId)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var patinet = await _context.Patients
                .Include(p => p.nutritionist)
                .Where(p => p.UserId == userId).FirstOrDefaultAsync();

            var timeSelected = await _context.AvailableTime
      .Where(e => e.Id == timeId)
      .Select(e => new { e.DayOfWeek, e.Time })
      .FirstOrDefaultAsync();

            var timeS = await _context.AvailableTime
     .Where(e => e.Id == timeId)
     .FirstOrDefaultAsync();

            var currentTime = DateTime.Now.Date;


            int daysUntilSelectedDay = ((int)timeSelected.DayOfWeek - (int)currentTime.DayOfWeek + 7) % 7;
            if (daysUntilSelectedDay == 0)
            {
                daysUntilSelectedDay = 7;
            }

            var appointmentDateTime = currentTime.AddDays(daysUntilSelectedDay).Add(timeSelected.Time);

            var appoitment = new Appoitment();
            appoitment.Status = "Pending";

            appoitment.PatientId = patinet.Id;
            appoitment.Time = appointmentDateTime; // Assign the calculated appointment date and time
            if (patinet.NutritionistId == null || patinet.NutritionistId == timeS.NutritionistId)
            {
                appoitment.NutritionistId = timeS.NutritionistId;
            }
            else
            {
                throw new Exception($"you have Nutritionist {patinet.nutritionist.Name}");
            }
            await _context.Appoitments.AddAsync(appoitment);
            await _context.SaveChangesAsync();

            return appoitment;
        }

        /// <summary>
        /// Updates an existing appointment with new details.
        /// </summary>
        /// <param name="id">The ID of the appointment to update.</param>
        /// <param name="appoitmentDto">The updated details of the appointment.</param>


        public async Task Put(int id, AppoitmentDto appoitmentDto)
        {
            var appoitmentToUpdata = await _context.Appoitments.Where(a => a.Id == id).FirstOrDefaultAsync();
            appoitmentToUpdata.Time = appoitmentDto.Time;
            appoitmentToUpdata.Status = appoitmentDto.Status;


            await _context.SaveChangesAsync();
        }

        public Task<List<ViewAppointment>> GetMyById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<ViewAppointment>> GetMyByIdForPatient(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Count()
        {
            return await _context.Appoitments.CountAsync();
        }
    }
}
