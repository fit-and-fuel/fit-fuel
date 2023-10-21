using Azure.Storage.Blobs.Models;
using Azure.Storage.Blobs;
using fit_and_fuel.Data;
using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace fit_and_fuel.Services
{
    public class PatientService : IPatients
    {
        private UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;


        private INotification _notificationService;
        public PatientService(AppDbContext context, INotification notificationService, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;

            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Deletes a patient record based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the patient to be deleted.</param>

        public async Task Delete(int id)
        {
            var patient = await _context.Patients.SingleOrDefaultAsync(x => x.Id == id);
            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Selects a nutritionist for a patient.
        /// </summary>
        /// <param name="NutId">The ID of the selected nutritionist.</param>
        /// <param name="UserId">The ID of the patient.</param>

        public async Task SelectNut(int NutId, string UserId)
        {
            var patient = await _context.Patients.SingleOrDefaultAsync(x => x.UserId == UserId);
            patient.NutritionistId = NutId;
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves a list of all patients.
        /// </summary>
        /// <returns>A list of Patient objects representing all patients.</returns>

        public async Task<List<Patient>> GetAll()
        {
            var patients = await _context.Patients
            //.Include(p => p.appoitments)
            //.Include(p => p.dietPlan)
            //.Include(p => p.nutritionist)
            .ToListAsync();
            return patients;
        }



        public async Task<List<PatientDtoView>> GetAllDto()
        {
            var patients = await GetAll();
            var patientsToReturn = patients.Select(patients => new PatientDtoView
            {
                Id = patients.Id,
                Name = patients.Name,
                UserId = patients.UserId,
                Gender = patients.Gender,
                Age = patients.Age,
                PhoneNumber = patients.PhoneNumber,
                imgURl = patients.imgURl,
                NutritionistId = patients.NutritionistId
            }).ToList();
            return patientsToReturn;

        }
        /// <summary>
        /// Retrieves a specific patient by their user ID.
        /// </summary>
        /// <param name="id">The user ID of the patient to retrieve.</param>
        /// <returns>A Patient object representing the specified patient.</returns>


        public async Task<Patient> GetById(int id)
        {            //.Include(p => p.appoitments)
                     //.Include(p => p.dietPlan)

            var patient = await _context.Patients
                 .Include(p => p.nutritionist)
                .Where(p => p.Id == id)

                .FirstOrDefaultAsync();
            return patient;
        }
        public async Task<PatientDtoView> GetByIdDto(int id)
        {

            var patient = await GetById(id);
            var patientsToReturn = new PatientDtoView
            {
                Id = patient.Id,
                Name = patient.Name,
                UserId = patient.UserId,
                Gender = patient.Gender,
                Age = patient.Age,
                PhoneNumber = patient.PhoneNumber,
                imgURl = patient.imgURl,
                NutritionistId = patient.NutritionistId
            };
            return patientsToReturn;

        }

        /// <summary>
        /// Creates a new patient record based on the provided data.
        /// </summary>
        /// <param name="resId">The user ID associated with the patient.</param>
        /// <param name="patientDto">DTO containing patient information.</param>
        /// <returns>A newly created Patient object.</returns>

        public async Task<Patient> Post(PatientDto patientDto, IFormFile file)
        {

            var imageUrl = await UploadFile(file);
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var patientExists = await GetAll();
            var patientList = patientExists.Where(p => p.UserId == userId).FirstOrDefault();
            var user = await _userManager.FindByIdAsync(userId);
            // var userName = user.UserName;
            var userPhonenumber = user.PhoneNumber;

            if (patientList == null)
            {
                var patient = new Patient()
                {
                    UserId = userId,
                    Name = patientDto.Name,
                    Gender = patientDto.Gender,
                    Age = patientDto.Age,

                    PhoneNumber = userPhonenumber,
                    //NutritionistId = patientDto.NutritionistId,

                    imgURl = imageUrl
                };
                await _context.Patients.AddAsync(patient);
                await _context.SaveChangesAsync();

                return patient;
            }
            return null;
        }

        public async Task<string> UploadFile(IFormFile file)
        {
            var URL = "https://ecommerceprojectimages.blob.core.windows.net/images/noimage.png";
            if (file != null)
            {
                BlobContainerClient blobContainerClient =
                    new BlobContainerClient(_configuration.GetConnectionString("StorageAccount"), "images");

                await blobContainerClient.CreateIfNotExistsAsync();

                BlobClient blobClient = blobContainerClient.GetBlobClient(file.FileName);

                using var fileStream = file.OpenReadStream();

                BlobUploadOptions blobUploadOptions = new BlobUploadOptions()
                {
                    HttpHeaders = new BlobHttpHeaders { ContentType = file.ContentType }
                };

                if (!blobClient.Exists())
                {
                    await blobClient.UploadAsync(fileStream, blobUploadOptions);
                }
                URL = blobClient.Uri.ToString();
            }
            return URL;
        }


        /// <summary>
        /// Updates the information of a specific patient.
        /// </summary>
        /// <param name="id">The ID of the patient to update.</param>
        /// <param name="patientDto">DTO containing updated patient information.</param>

        public async Task Put(int id, PatientDto patientDto)
        {
            var patient = await _context.Patients.SingleOrDefaultAsync(x => x.Id == id);

            patient.Name = patientDto.Name;
            patient.Gender = patientDto.Gender;
            patient.Age = patientDto.Age;

            patient.PhoneNumber = patientDto.PhoneNumber;
            //patient.NutritionistId = patientDto.NutritionistId;
            patient.imgURl = patientDto.imgURl;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Searches for patients by a specific search string.
        /// </summary>
        /// <param name="str">The search string to match against patient names.</param>
        /// <returns>A list of Patient objects matching the search criteria.</returns>

        public async Task<List<Patient>> Search(string str)
        {
            var pat = await _context.Patients.Where(e => e.Name.Contains(str)).ToListAsync();
            return pat;
        }
        public async Task<List<PatientDtoView>> SearchDto(string str)
        {
            var patients = await Search(str);
            var patientsToReturn = patients.Select(patients => new PatientDtoView
            {
                Id = patients.Id,
                Name = patients.Name,
                UserId = patients.UserId,
                Gender = patients.Gender,
                Age = patients.Age,
                PhoneNumber = patients.PhoneNumber,
                imgURl = patients.imgURl,
                NutritionistId = patients.NutritionistId
            }).ToList();
            return patientsToReturn;
        }
        /// <summary>
        /// Retrieves the diet plan of the currently assigned nutritionist for a specific patient.
        /// </summary>
        /// <param name="UserId">The ID of the patient.</param>
        /// <returns>The DietPlan object associated with the patient's nutritionist.</returns>

        public async Task<DietPlan> GetMyDietPlan()
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var dietPlan = await _context.Patients
       .Where(n => n.UserId == userId)
       .Include(d => d.dietPlan)
       .ThenInclude(d => d.days)
       .ThenInclude(d => d.meals)
       .Select(n => n.dietPlan)

       .FirstOrDefaultAsync();

            return dietPlan;
        }
        public async Task<DietPlanDtoView> GetMyDietPlanDto(string UserId)
        {
            var dietplan = await GetMyDietPlan();
            if (dietplan != null)
            {
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
            return null;
        }
        /// <summary>
        /// Retrieves a list of meals for the current day's diet plan of a patient.
        /// </summary>
        /// <param name="UserId">The ID of the patient.</param>
        /// <returns>A list of Meal objects representing the patient's meals for the current day.</returns>


        public async Task<List<Meal>> GetMyMealsForToday()
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var myDietPlan = await GetMyDietPlan();

            // Find the meals for the current day and flatten the structure
            var mealsForToday = myDietPlan.days
                .Where(d => d.Date.Date == DateTime.Now.Date)
                .SelectMany(d => d.meals)
                .ToList();

            // Return only the list of meals without the associated day and diet plan information
            var mealsOnly = mealsForToday.Select(meal => new Meal
            {
                Id = meal.Id,
                Name = meal.Name,
                Description = meal.Description,
                Calories = meal.Calories,
                Completion = meal.Completion,
                DayId = meal.DayId
            }).ToList();

            return mealsOnly;
        }


        /// <summary>
        /// Marks a specific meal as completed for the current day's diet plan of a patient.
        /// </summary>
        /// <param name="UserId">The ID of the patient.</param>
        /// <param name="MealId">The ID of the meal to mark as completed.</param>

        public async Task MealIsCompletion(int MealId)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var mealToday = await GetMyMealsForToday();
            var mealToUpdate = mealToday.FirstOrDefault(m => m.Id == MealId);
            var patient = await _context.Patients.Where(p => p.UserId == userId).FirstOrDefaultAsync();
            if (mealToUpdate == null)
            {
                throw new Exception("this meal is not Today");
            }
            var meal = await _context.Meals
                .FindAsync(MealId);

            if (meal != null)
            {
                meal.Completion = true;
                var nut = await _context.Patients
                    .Include(p => p.nutritionist)
                    .FirstOrDefaultAsync(p => p.UserId == userId);

                var content = new NotificationDto()
                {
                    Content = $"Your patient {patient.Name}  with UserId: {userId}  take meal {meal.Name} with id {meal.Id} "
                };

                await _notificationService.SendNotification(nut.nutritionist.UserId.ToString(), content);

                await _context.SaveChangesAsync();
            }
        }



        public Task SelectNut(int NutId, int UserId)
        {
            throw new NotImplementedException();
        }

        public Task<DietPlan> GetMyDietPlan(int UserId)
        {
            throw new NotImplementedException();
        }

        public Task<DietPlanDtoView> GetMyDietPlanDto(int UserId)
        {
            throw new NotImplementedException();
        }

        public Task<List<Meal>> GetMyMealsForToday(int UserId)
        {
            throw new NotImplementedException();
        }

        public Task MealIsCompletion(int UserId, int MealId)
        {
            throw new NotImplementedException();
        }

        public Task<Patient> Post(int resId, PatientDto patientDto)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Count()
        {
            return await _context.Patients.CountAsync();
        }

        public async Task<Patient> GetMyProfile()
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var myprofile = await _context.Patients
                .Where(p => p.UserId == userId)
                .Include(p=>p.nutritionist)
                .ThenInclude(b=>b.Price)
                .FirstOrDefaultAsync();
            return myprofile;

        }
    }
}
