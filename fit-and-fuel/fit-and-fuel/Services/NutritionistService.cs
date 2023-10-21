using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using fit_and_fuel.Data;
using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
using Microsoft.AspNetCore.Identity;
//using Humanizer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Policy;
//using static NuGet.Packaging.PackagingConstants;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace fit_and_fuel.Services
{
    /// <summary>
    /// Service class responsible for managing nutritionist-related operations and interactions with the database.
    /// </summary>
    public class NutritionistService : INutritionists
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;



        public NutritionistService(AppDbContext context, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
            _configuration = configuration;
        }

        /// <summary>
        /// Deletes a nutritionist record based on the provided ID.
        /// </summary>
        /// <param name="id">The ID of the nutritionist to be deleted.</param>

        public async Task Delete(int id)
        {
            var nut = await _context.Nutritionists.Where(n => n.Id == id).FirstOrDefaultAsync();
            _context.Nutritionists.Remove(nut);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Retrieves a list of all nutritionists.
        /// </summary>
        /// <returns>A list of Nutritionist objects representing all nutritionists.</returns>

        public async Task<List<Nutritionist>> GetAll()
        {

            var nuts = await _context.Nutritionists
            .Include(p => p.patients)
                .Include(c => c.clinic)
                .Include(n => n.Ratings)

            .ToListAsync();
           
            return nuts;
        }
        public async Task<List<NutritionistDtoView>> GetAllDto()
        {
            var nutritionists = await GetAll();
       
            
            var nutritionistDtoViews = nutritionists.Select(nutritionist => new NutritionistDtoView
            {
                Id = nutritionist.Id,
                Name = nutritionist.Name,
                UserId = nutritionist.UserId,
                Gender = nutritionist.Gender,
                Age = nutritionist.Age,
                PhoneNumber = nutritionist.PhoneNumber,
                CvURl = nutritionist.CvURl,
                imgURl = nutritionist.imgURl,
                AverageRating = CalculateAverageRating(nutritionist.Ratings)
            }).ToList();

            return nutritionistDtoViews;
        }
        private double? CalculateAverageRating(ICollection<Rating> ratings)
        {
            if (ratings == null || !ratings.Any())
            {
                return null;
            }

            int sum = 0;
            int count = 0;

            foreach (var rating in ratings)
            {
                if (rating.Value != 0) 
                {
                    sum += rating.Value;
                    count++;
                }
            }

            if (count == 0)
            {
                return null;
            }

            return (double)sum / count; 
        }


        /// <summary>
        /// Retrieves a list of all patients associated with a specific nutritionist.
        /// </summary>
        /// <param name="UserId">The ID of the nutritionist.</param>
        /// <returns>A list of Patient objects associated with the specified nutritionist.</returns>
        public async Task<List<Patient>> GetAllMyPatient(string UserId)
          {
        var patients = await _context.Nutritionists
        .Where(n => n.UserId == UserId)
        .SelectMany(n => n.patients)
        .ToListAsync();
        return patients;
         }
        public async Task<List<PatientDtoViewNut>> GetAllMyPatientDto(string UserId)
        {
            var patients = await GetAllMyPatient(UserId);
            var patientsToReturn = patients.Select(patient=> new PatientDtoViewNut
            {
                Id = patient.Id,
                Name = patient.Name,
                UserId = patient.UserId,
                Gender = patient.Gender,
                Age = patient.Age,
                PhoneNumber = patient.PhoneNumber,
                imgURl = patient.imgURl,
                
            }).ToList();
            return patientsToReturn;
        }


        /// <summary>
        /// Retrieves a list of all diet plans created by a specific nutritionist.
        /// </summary>
        /// <param name="UserId">The ID of the nutritionist.</param>
        /// <returns>A list of DietPlan objects created by the specified nutritionist.</returns>

        public async Task<List<DietPlan>> GetAllDietPlan(string UserId)
        {
            var dietPlans = await _context.Nutritionists
       .Where(n => n.UserId == UserId)
       .SelectMany(n => n.dietPlans)
       .Include(d=>d.days)
       .ThenInclude(d=>d.meals)
       .ToListAsync();

            return dietPlans;
        }
        public async Task<List<DietPlanDtoView>> GetAllDietPlanDto(string UserId)
        {
            var dietplans = await GetAllDietPlan(UserId);
            var dietplansToReturn = dietplans.Select(dietplan => new DietPlanDtoView
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
            }).ToList();
            return dietplansToReturn;
        }
        /// <summary>
        /// Retrieves a specific nutritionist by their ID.
        /// </summary>
        /// <param name="id">The ID of the nutritionist to retrieve.</param>
        /// <returns>A Nutritionist object representing the specified nutritionist.</returns>

        public async Task<Nutritionist> GetById(int id)
        {
            var nut = await _context.Nutritionists
            .Include(p => p.patients)
             .Include(n => n.Ratings)
            .Include(c => c.clinic)
            .Include(a=>a.appoitments)
            .Include(AT => AT.AvaliableTimes)
            .Where(n => n.Id == id).FirstOrDefaultAsync();
            return nut;
        }
        public async Task<NutritionistDtoView> GetByIdDto(int id)
        {
            var nutritionist = await GetById(id);
            var nut = new NutritionistDtoView()
            {
                Id = nutritionist.Id,
                Name = nutritionist.Name,
                UserId = nutritionist.UserId,
                Gender = nutritionist.Gender,
                Age = nutritionist.Age,
                PhoneNumber = nutritionist.PhoneNumber,
                CvURl = nutritionist.CvURl,
                imgURl = nutritionist.imgURl,
                AverageRating = CalculateAverageRating(nutritionist.Ratings)
            };
            return nut;
        }
        /// <summary>
        /// Retrieves the profile information of a specific nutritionist.
        /// </summary>
        /// <param name="id">The ID of the nutritionist.</param>
        /// <returns>A Nutritionist object containing profile information.</returns>

        public async Task<Nutritionist> GetMyProfile()
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var nut = await _context.Nutritionists
                .Include(p => p.patients)
                .ThenInclude(d => d.dietPlan)
                .Include(n=>n.AvaliableTimes)
           .Where(n => n.UserId== userId).FirstOrDefaultAsync();
            return nut;
        }
        public async Task<NutritionistDtoView> GetMyProfileDto(string id)
        {
            var nutritionist = await GetMyProfile();
            var nut = new NutritionistDtoView()
            {
                Id = nutritionist.Id,
                Name = nutritionist.Name,
                UserId = nutritionist.UserId,
                Gender = nutritionist.Gender,
                Age = nutritionist.Age,
                PhoneNumber = nutritionist.PhoneNumber,
                CvURl = nutritionist.CvURl,
                imgURl = nutritionist.imgURl,
                AverageRating = CalculateAverageRating(nutritionist.Ratings)
            };
            return nut;
        }
      
        /// <summary>
        /// Creates a new nutritionist record based on the provided data.
        /// </summary>
        /// <param name="UserId">The ID of the user associated with the nutritionist.</param>
        /// <param name="nutritionistDto">DTO containing nutritionist information.</param>
        /// <returns>A newly created Nutritionist object.</returns>

        public async Task<Nutritionist> Post( NutritionistDto nutritionistDto, IFormFile file, IFormFile cvfile)
        {
            var imageUrl = await UploadFile(file);
            var cv = await UploadFileCv(cvfile);

            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.FindByIdAsync(userId);
			var NutList = _context.Nutritionists.Where(p => p.UserId == userId).FirstOrDefault();

            if (NutList == null)
            {
				var nut = new Nutritionist()
				{
					UserId = userId,
					PhoneNumber = user.PhoneNumber,
					Name = nutritionistDto.Name,
					Gender = nutritionistDto.Gender,
					Age = nutritionistDto.Age,
					CvURl = cv,
					imgURl = imageUrl
                };
				await _context.Nutritionists.AddAsync(nut);
				await _context.SaveChangesAsync();

				return nut;
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
        ///////////////////////////////////// for cv
		public async Task<string> UploadFileCv(IFormFile file)
		{
            //var URL = "https://ecommerceprojectimages.blob.core.windows.net/images/noimage.png";
            var URL = "";
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
			
			return URL;
		}

		/// <summary>
		/// Updates the profile information of a specific nutritionist.
		/// </summary>
		/// <param name="id">The ID of the nutritionist to update.</param>
		/// <param name="nutritionistDto">DTO containing updated nutritionist information.</param>

		public async Task Put(int id, NutritionistDto nutritionistDto)
        {
            var nut = await _context.Nutritionists.Where(n => n.Id == id).FirstOrDefaultAsync();
            nut.PhoneNumber = nutritionistDto.PhoneNumber;
            nut.Name = nutritionistDto.Name;
            nut.Gender = nutritionistDto.Gender;
            nut.Age = nutritionistDto.Age;
            nut.CvURl = nutritionistDto.CvURl;
            nut.imgURl = nutritionistDto.imgURl;

            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Searches for nutritionists by a specific search string.
        /// </summary>
        /// <param name="str">The search string to match against nutritionist names.</param>
        /// <returns>A list of Nutritionist objects matching the search criteria.</returns>

        public async Task<List<Nutritionist>> Search(string str)
        {
            var nut = await _context.Nutritionists
                .Include(p => p.patients)
            .Include(c => c.clinic)
                .Where(e => e.Name.Contains(str)).ToListAsync();
            return nut;
        }
        public async Task<List<NutritionistDtoView>> SearchDto(string str)
        {
            var nutritionists = await Search(str);

            var nutritionistDtoViews = nutritionists.Select(nutritionist => new NutritionistDtoView
            {
                Id = nutritionist.Id,
                Name = nutritionist.Name,
                UserId = nutritionist.UserId,
                Gender = nutritionist.Gender,
                Age = nutritionist.Age,
                PhoneNumber = nutritionist.PhoneNumber,
                CvURl = nutritionist.CvURl,
                imgURl = nutritionist.imgURl,
                AverageRating = CalculateAverageRating(nutritionist.Ratings)
            }).ToList();

            return nutritionistDtoViews;
        }

        public Task<List<Patient>> GetAllMyPatient(int UserId)
        {
            throw new NotImplementedException();
        }

        public Task<List<PatientDtoViewNut>> GetAllMyPatientDto(int UserId)
        {
            throw new NotImplementedException();
        }

        public Task<List<DietPlan>> GetAllDietPlan(int UserId)
        {
            throw new NotImplementedException();
        }

        public Task<List<DietPlanDtoView>> GetAllDietPlanDto(int UserId)
        {
            throw new NotImplementedException();
        }

        public Task<Nutritionist> GetMyProfile(int id)
        {
            throw new NotImplementedException();
        }

        public Task<NutritionistDtoView> GetMyProfileDto(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Nutritionist> Post(int UserId, NutritionistDto nutritionistDto)
        {
            throw new NotImplementedException();
        }

		public async Task<int> Count()
		{
			return await _context.Nutritionists.CountAsync();
		}

		public Task<Nutritionist> Post(NutritionistDto nutritionistDto)
		{
			throw new NotImplementedException();
		}
	}
}
