using fit_and_fuel.Data;
using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace fit_and_fuel.Services
{
    public class DayService : IDays
    {

        private readonly AppDbContext _context;

        public DayService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Deletes a day by its ID.
        /// </summary>
        /// <param name="id">The ID of the day to delete.</param>

        public async Task Delete(int id)
        {
            var day = await _context.Days.Where(day => day.Id == id).FirstOrDefaultAsync();
            _context.Days.Remove(day);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves a list of all days along with their meals.
        /// </summary>
        /// <returns>A list of all days with their associated meals.</returns>

        public async Task<List<Day>> GetAll()
        {
            return await _context.Days
                .Include(p => p.meals)
                .ToListAsync();
        }
        //public int Id { get; set; }

        //public string DayName { get; set; }

        //public DateTime Date { get; set; }

        //public int TotalCalories { get; set; }
        //public int DietPlanId { get; set; }
        //public ICollection<MealDtoView> meals { get; set; }
        public async Task<List<DayDtoView>> GetAllDto()
        {
            var days = await GetAll();
            var daysToReturn = days.Select(day =>new DayDtoView
            {
                Id = day.Id,
                DayName =day.DayName,
                Date = day.Date,
                TotalCalories = day.TotalCalories,
                DietPlanId= day.DietPlanId,
                meals =day.meals.Select(meal =>new MealDtoView()
                {
                    Id = meal.Id,
                    Name = meal.Name,
                    DayId = meal.DayId,
                    Description = meal.Description,
                    Calories = meal.Calories,
                    Completion = meal.Completion,
                }).ToList()
            }
            ).ToList();
            return daysToReturn;
        }

        /// <summary>
        /// Retrieves a day by its ID along with its meals.
        /// </summary>
        /// <param name="id">The ID of the day to retrieve.</param>
        /// <returns>The day with the specified ID along with its associated meals.</returns>

        public async Task<Day> GetById(int id)
        {

            var days= await _context.Days
                .Include(p => p.meals)
                .Where(day => day.Id == id).FirstOrDefaultAsync();
            return days;

        }

        public async Task<DayDtoView> GetByIdDto(int id)
        {
            var day = await GetById(id);
            var dayToReturn = new DayDtoView
            {
                Id = day.Id,
                DayName = day.DayName,
                Date = day.Date,
                TotalCalories = day.TotalCalories,
                DietPlanId = day.DietPlanId,
                meals = day.meals.Select(meal => new MealDtoView()
                {
                    Id = meal.Id,
                    Name = meal.Name,
                    DayId = meal.DayId,
                    Description = meal.Description,
                    Calories = meal.Calories,
                    Completion = meal.Completion,
                }).ToList()
            };
            return dayToReturn;
        }

        /// <summary>
        /// Creates a new day with the specified details.
        /// </summary>
        /// <param name="day">The details of the day to create.</param>
        /// <returns>The newly created day.</returns>

        public async Task<Day> Post(DayDto day)
        {
            var dietPlan = await _context.DietPlans.FirstOrDefaultAsync(d => d.Id == day.DietPlanId);

            var nameday = day.Date.DayOfWeek.ToString();

            var NewDay = new Day()
            {
                DayName = nameday,
                Date = day.Date,
                DietPlanId = day.DietPlanId,
            };

            if (NewDay.Date < dietPlan.StartDate || NewDay.Date > dietPlan.EndDate)
            {
                throw new InvalidOperationException("Day's date is outside the Diet Plan's duration");
            }

            await _context.AddAsync(NewDay);
            await _context.SaveChangesAsync();

            await _context.SaveChangesAsync();

            return NewDay;
        }

        public Task<DayDtoView> PostDto(DayDto day)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Updates the DietPlanId of a day.
        /// </summary>
        /// <param name="id">The ID of the day to update.</param>
        /// <param name="day">The updated details of the day.</param>

        public async Task Put(int id, DayDto day)
        {
            var updateday = await _context.Days.Where(m => m.Id == id).FirstOrDefaultAsync();

            updateday.DietPlanId = day.DietPlanId;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Calculates the total calories for a specific day based on its associated meals.
        /// </summary>
        /// <param name="dayId">The ID of the day for which to calculate total calories.</param>
        /// <returns>The total calories for the specified day.</returns>
        
        private async Task<int> CalculateTotalCaloriesForDay(int dayId)
        {
            var meals = await _context.Meals.Where(m => m.DayId == dayId).ToListAsync();
            return meals.Sum(m => m.Calories);
        }
    }
}
