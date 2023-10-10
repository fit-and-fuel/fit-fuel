using fit_and_fuel.Data;
using fit_and_fuel.DTOs;
using fit_and_fuel.DTOs.View.ViewAvilableTme;
using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
using Microsoft.EntityFrameworkCore;

namespace fit_and_fuel.Services
{
    public class AvailableTimeService : IAvailableTime
    {

        private readonly AppDbContext _context;

        public AvailableTimeService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Deletes an available time slot for a nutritionist.
        /// </summary>
        /// <param name="UserId">The ID of the nutritionist user.</param>
        /// <param name="id">The ID of the available time slot to delete.</param>

        public async Task Delete(int UserId, int id)
        {
            var nut = await _context.Nutritionists
                .Include(n=>n.AvaliableTimes)
                .FirstOrDefaultAsync(x => x.UserId == UserId);
            var nutTimes= nut.AvaliableTimes
                .Where(a=>a.Id == id)
                .FirstOrDefault();
            if (nutTimes != null)
            {
                var timeToRemove=await _context.AvailableTime.FirstOrDefaultAsync(a=>a.Id==id);
                _context.AvailableTime.Remove(timeToRemove);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("You dont have this Time");
            }

        }

        /// <summary>
        /// Retrieves a list of all available time slots for a nutritionist.
        /// </summary>
        /// <param name="NutritionistId">The ID of the nutritionist.</param>
        /// <returns>A list of available time slots.</returns>

        public async Task<List<AvailableTime>> GetAll(int NutritionistId)
        {
         
            var availableTimes = await _context.AvailableTime
                .Where(a=>a.NutritionistId == NutritionistId)
                .ToListAsync();
            return availableTimes;
        }

        public async Task<List<ViewAvilableTime>> GetAllDto(int NutritionistId)
        {
            var AvilableTimes = await GetAll(NutritionistId);
            var AvilableTimeDto = AvilableTimes.Select(x => new ViewAvilableTime
            {
                Id = x.Id,
                DayOfWeek = x.DayOfWeek,
                Time = x.Time
            }).ToList();

            return AvilableTimeDto;
        }


        /// <summary>
        /// Creates a new available time slot for a nutritionist.
        /// </summary>
        /// <param name="UserId">The ID of the nutritionist user.</param>
        /// <param name="availableTimeDto">The details of the available time slot to create.</param>
        


        public async Task Post(int UserId,AvailableTimeDto availableTimeDto)
        {
            var nut = await _context.Nutritionists.FirstOrDefaultAsync(x => x.UserId == UserId);
            var avaiTime = new AvailableTime()
            {
                NutritionistId = nut.Id,
                DayOfWeek = availableTimeDto.DayOfWeek,
                Time = availableTimeDto.Time,
            };
            await _context.AvailableTime.AddAsync(avaiTime);
            await _context.SaveChangesAsync();
         }

        /// <summary>
        /// Updates an existing available time slot for a nutritionist.
        /// </summary>
        /// <param name="UserId">The ID of the nutritionist user.</param>
        /// <param name="availableTimeDto">The updated details of the available time slot.</param>
        /// <param name="id">The ID of the available time slot to update.</param>

        public async Task Put(int UserId, AvailableTimeDto availableTimeDto, int id)
        {
            var nut = await _context.Nutritionists
               .Include(n => n.AvaliableTimes)
               .FirstOrDefaultAsync(x => x.UserId == UserId);
            var nutTimes = nut.AvaliableTimes
                .Where(a => a.Id == id)
                .FirstOrDefault();
            var timeToUpdata = await _context.AvailableTime.FirstOrDefaultAsync(a => a.Id == id);
            if (nutTimes != null && timeToUpdata != null)
            {
                
                timeToUpdata.DayOfWeek = availableTimeDto.DayOfWeek;
                timeToUpdata .Time = availableTimeDto.Time;
                
               
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("You dont have this Time");
            }
        }
    }
}
