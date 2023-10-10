using fit_and_fuel.Data;
using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace fit_and_fuel.Services
{
    /// <summary>
    /// Service class responsible for handling operations related to meals within a day.
    /// </summary>
    public class MealService : IMeals
    {
        private readonly AppDbContext _context;

        public MealService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Deletes a meal based on its ID.
        /// </summary>
        /// <param name="id">The ID of the meal to be deleted.</param>

        public async Task Delete(int id)
        {
            var meal = await _context.Meals.Where(m => m.Id == id).FirstOrDefaultAsync();
            _context.Meals.Remove(meal);
            await _context.SaveChangesAsync();

        }

        /// <summary>
        /// Retrieves a list of all meals.
        /// </summary>
        /// <returns>A list of all meals.</returns>

        public async Task<List<Meal>> GetAll()
        {
            var meals = await _context.Meals
                .Include(m => m.day)
                .ToListAsync();
            return meals;
        }
    
        public async Task<List<MealDtoView>> GetAllDto()
        {
            var meals = await GetAll();
            var mealsToReturn = meals.Select(meal => new MealDtoView
            {
            Id = meal.Id,
            Name = meal.Name,
            DayId = meal.DayId,
            Description = meal.Description,
            Calories = meal.Calories,
            Completion = meal.Completion,
            }
            ).ToList();
            return mealsToReturn;
        }

        /// <summary>
        /// Retrieves a specific meal based on its ID.
        /// </summary>
        /// <param name="id">The ID of the meal to retrieve.</param>
        /// <returns>The retrieved meal.</returns>


        public async Task<Meal> GetById(int id)
        {
            var meal = await _context.Meals
                .Include(m => m.day)
                .Where(m => m.Id == id).FirstOrDefaultAsync();
            return meal;
        }

        public async Task<MealDtoView> GetByIdDto(int id)
        {
            var meal = await GetById(id);
            var mealsToReturn =  new MealDtoView
            {
                Id = meal.Id,
                Name = meal.Name,
                DayId = meal.DayId,
                Description = meal.Description,
                Calories = meal.Calories,
                Completion = meal.Completion,
            };
            return mealsToReturn;

        }

        /// <summary>
        /// Adds a new meal to a day.
        /// </summary>
        /// <param name="meal">The DTO containing meal information.</param>
        /// <returns>The newly added meal.</returns>

        public async Task<Meal> Post(MealDto meal)
        {
            var newMeal = new Meal()
            {
                Calories = meal.Calories,
                Name = meal.Name,
                DayId = meal.DayId,
                Description = meal.Description,
                Completion = meal.Completion,



            };

            await _context.Meals.AddAsync(newMeal);

            await _context.SaveChangesAsync();
            return newMeal; 
        }

        public async Task<MealDtoView> PostDto(MealDto meal)
        {
            var newMeal = new Meal()
            {
                Calories = meal.Calories,
                Name = meal.Name,
                DayId = meal.DayId,
                Description = meal.Description,
                Completion = meal.Completion,



            };

            await _context.Meals.AddAsync(newMeal);

            await _context.SaveChangesAsync();
            return await GetByIdDto(newMeal.Id);
        }

        /// <summary>
        /// Updates a meal's information based on its ID.
        /// </summary>
        /// <param name="id">The ID of the meal to update.</param>
        /// <param name="meal">The DTO containing updated meal information.</param>

        public async Task Put(int id, MealDto meal)
        {
            var toupdatemeal = await _context.Meals.Where(m => m.Id == id).FirstOrDefaultAsync();


            toupdatemeal.Calories = meal.Calories;
            toupdatemeal.Name = meal.Name;
            toupdatemeal.DayId = meal.DayId;
            toupdatemeal.Description = meal.Description;
            toupdatemeal.Completion = meal.Completion;

            await _context.SaveChangesAsync();

        }
    }
}
