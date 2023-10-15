using fit_and_fuel.Data;
using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;

using fit_and_fuel.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace fit_and_fuel.Services
{
    public class DietPlanService : IDietPlan
    {
        private INotification _notificationService;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public DietPlanService(AppDbContext context, INotification notificationService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            

            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Deletes a diet plan by its ID.
        /// </summary>
        /// <param name="id">The ID of the diet plan to delete.</param>

        public async Task Delete(int id)
        {
            var dietplan = await _context.DietPlans.Where(d => d.Id == id).FirstOrDefaultAsync();

            _context.DietPlans.Remove(dietplan);

            await _context.SaveChangesAsync();

        }

        /// <summary>
        /// Retrieves a list of all diet plans along with their associated days and meals.
        /// </summary>
        /// <returns>A list of all diet plans with their associated days and meals.</returns>

        public async Task<List<DietPlan>> GetAll()
        {
            var dietplans = await _context.DietPlans
                .Include(d=>d.days).ThenInclude(m=>m.meals)
                
                .ToListAsync();

            return dietplans;
        }
        //public int Id { get; set; }
        //public int PatientId { get; set; }
        //public int NutritionistId { get; set; }
        //public int Duration { get; set; }
        //public DateTime StartDate { get; set; }
        //public DateTime EndDate { get; set; }
        //public ICollection<DayDtoView> days { get; set; }
        public async Task<List<DietPlanDtoView>> GetAllDto()
        {
            var dietplans = await GetAll();
            var dietplansToReturn = dietplans.Select(dietplan=>new DietPlanDtoView
            {
                Id = dietplan.Id,
                PatientId = dietplan.PatientId,
                NutritionistId = dietplan.NutritionistId,
                Duration = dietplan.Duration,
                StartDate = dietplan.StartDate,
                EndDate = dietplan.EndDate,
                days =dietplan.days.Select(day => new DayDtoView
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
                }
            ).ToList()
            }).ToList();
            return dietplansToReturn;
        }
        /// <summary>
        /// Retrieves a diet plan by its ID along with its associated days and meals.
        /// </summary>
        /// <param name="id">The ID of the diet plan to retrieve.</param>
        /// <returns>The diet plan with the specified ID along with its associated days and meals.</returns>

        public async Task<DietPlan> GetById(int id)
        {
            var dietplan = await _context.DietPlans
                .Include(p => p.nutritionist)
                .Include(d => d.Patient)
                 .Include(d => d.days).ThenInclude(m => m.meals)
                .Where(d => d.Id == id).FirstOrDefaultAsync();

            return dietplan;
        }
        public async Task<DietPlanDtoView> GetByIdDto(int id)
        {
            var dietplan = await GetById(id);
            var dietplanToReturn = new DietPlanDtoView
            {
                Id = dietplan.Id,
                PatientId = dietplan.PatientId,
                NutritionistId = dietplan.NutritionistId,
                Duration = dietplan.Duration,
                StartDate = dietplan.StartDate,
                EndDate = dietplan.EndDate,
                days = dietplan.days.Select(day => new DayDtoView
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
                }
            ).ToList()
            };
            return dietplanToReturn;
        }
        /// <summary>
        /// Creates a new diet plan with the specified details.
        /// </summary>
        /// <param name="dietPlan">The details of the diet plan to create.</param>
        /// <returns>The newly created diet plan.</returns>

        public async Task<DietPlan> Post(DietPlanDto dietPlan)
        {

            TimeSpan timeSpan = dietPlan.EndDate - dietPlan.StartDate;
            var newDietPlan = new DietPlan()
            {
                Duration = timeSpan.Days,
                StartDate= dietPlan.StartDate,
                EndDate=dietPlan.EndDate,
                PatientId = dietPlan.PatientId,
                NutritionistId = dietPlan.NutritionistId
            };

            await _context.DietPlans.AddAsync(newDietPlan);

            await _context.SaveChangesAsync();

            return newDietPlan;
        }
        public async Task<DietPlan> PostDietPlanWithDay(DietPlanDto dietPlan)
        {
                        string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var nut = await _context.Nutritionists
             .Where(n => n.UserId == userId)
             .FirstOrDefaultAsync();
            TimeSpan timeSpan = dietPlan.EndDate - dietPlan.StartDate;
            var newDietPlan = new DietPlan()
            {
                Duration = timeSpan.Days,
                StartDate = dietPlan.StartDate,
                EndDate = dietPlan.EndDate,
                PatientId = dietPlan.PatientId,
                NutritionistId = nut.Id
            };
            await _context.DietPlans.AddAsync(newDietPlan);

            await _context.SaveChangesAsync();
            Day dayToAdd;
            for (int i = 0; i < timeSpan.Days; i++)
            {
                dayToAdd = new Day()
                {
                    Date = newDietPlan.StartDate.AddDays(i),
                    DayName = newDietPlan.StartDate.AddDays(i).DayOfWeek.ToString(),
                    DietPlanId = newDietPlan.Id,
                };
                await _context.Days.AddAsync(dayToAdd);
                await _context.SaveChangesAsync();

            }
      


            return newDietPlan;
        }

        /// <summary>
        /// Creates a new diet plan with specified days and meals for a patient.
        /// </summary>
        /// <param name="dietPlanDays">The details of the diet plan with days and meals.</param>
        /// <param name="UserId">The user ID of the nutritionist creating the diet plan.</param>

        public async Task PostFullDietPlan(DietPlanDaysDto dietPlanDays,string UserId)
        {

            var nut=await _context.Nutritionists
                .Where(n=>n.UserId == UserId)
                .FirstOrDefaultAsync();
              var patientIds = await _context.Patients
        .Where(p => p.NutritionistId == nut.Id)
         .Select(p => p.Id)
        .ToListAsync();
            if (!patientIds.Contains(dietPlanDays.PatientId))
            {
                throw new Exception("You cannot add a diet plan for this patient.");
            }

            TimeSpan timeSpan = dietPlanDays.EndDate - dietPlanDays.StartDate;
            var dietPlan = new DietPlan()
            {
                StartDate = dietPlanDays.StartDate,
                EndDate = dietPlanDays.EndDate,
                Duration = timeSpan.Days,
                PatientId= dietPlanDays.PatientId,
                NutritionistId= nut.Id,
                days=new List<Day>()
            };
            Day dayToAdd;
            for (int i = 0; i < timeSpan.Days; i++)
            {
                dayToAdd = new Day()
                {
                    Date = dietPlanDays.StartDate.AddDays(i),
                    DayName = dietPlanDays.StartDate.AddDays(i).DayOfWeek.ToString(),
                    DietPlanId = dietPlan.Id,
                    meals = new List<Meal>()
                };
                var mealToAdd = dietPlanDays.Meals
                    .Where(m => m.DayNumber == i + 1)
                    .Select(m => new Meal
                    {
                        Name = m.Name,
                        Description = m.Description,
                        Calories = m.Calories,
                        DayId = dayToAdd.Id
                    })
                    .ToList();
                dayToAdd.meals = mealToAdd;
                dietPlan.days.Add(dayToAdd); 
            }
            _context.DietPlans.Add(dietPlan); // Add the diet plan to the context
            await _context.SaveChangesAsync();
            var pat = await _context.Patients.Where(p=>p.Id == dietPlan.PatientId).FirstOrDefaultAsync();
            
            var content = new NotificationDto()
            {
                Content = $"Your Have New DietPlan From Your Nutritionist {nut.Name}"
            };

            await _notificationService.SendNotification(pat.UserId.ToString(), content);

        }

        /// <summary>
        /// Updates a diet plan's patient and nutritionist IDs.
        /// </summary>
        /// <param name="id">The ID of the diet plan to update.</param>
        /// <param name="dietPlan">The updated details of the diet plan.</param>

        public async Task Put(int id, DietPlanDto dietPlan)
        {
            var ToUpdateDietPlan = await _context.DietPlans.Where(d => d.Id == id).FirstOrDefaultAsync();

            ToUpdateDietPlan.PatientId = dietPlan.PatientId;
            ToUpdateDietPlan.NutritionistId = dietPlan.NutritionistId;

            await _context.SaveChangesAsync();
        }

      

      

        public Task<DietPlanDtoView> PostDto(DietPlanDto dietPlan)
        {
            throw new NotImplementedException();
        }

        public Task PostFullDietPlan(DietPlanDaysDto dietPlanDays, int UserId)
        {
            throw new NotImplementedException();
        }

		public async Task<int> Count()
		{
			return await _context.DietPlans.CountAsync();
		}
	}


   
}


