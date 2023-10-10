using fit_and_fuel.Data;
using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
//using Humanizer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace fit_and_fuel.Services
{
    public class RatingService : IRating
    {
        private readonly AppDbContext _context;

        public RatingService(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of ratings for a specific nutritionist.
        /// </summary>
        /// <param name="nutritionistId">The ID of the nutritionist.</param>
        /// <returns>A list of ratings for the specified nutritionist.</returns>

        public async Task<List<Rating>> GetRatingsForNutritionist(int nutritionistId)
        {
            return await _context.Ratings
                .Where(r => r.NutritionistId == nutritionistId)
                .ToListAsync();
        }

        /// <summary>
        /// Adds a new rating and comment for a nutritionist given by a patient.
        /// </summary>
        /// <param name="patientId">The ID of the patient giving the rating.</param>
        /// <param name="ratingDto">DTO containing rating information.</param>

        public async Task AddRating(int patientId, RatingDto ratingDto)
        {
            var patient = await _context.Patients.Include(p => p.nutritionist).FirstOrDefaultAsync(p => p.UserId == patientId);
            if (patient == null || patient.nutritionist == null)
            {
                throw new InvalidOperationException("Patient or Nutritionist not found.");
            }

            var rating = new Rating
            {
                NutritionistId = patient.nutritionist.Id,
                PatientId = patientId,
                Value = ratingDto.Value,
                Comment = ratingDto.Comment
            };

            _context.Ratings.Add(rating);
            await _context.SaveChangesAsync();

        }

        /// <summary>
        /// Retrieves a list of nutritionists whose average rating falls within a specified range.
        /// </summary>
        /// <param name="ratingRange">The rating range to filter nutritionists.</param>
        /// <returns>A list of nutritionists with average ratings within the specified range.</returns>

        public async Task<List<Nutritionist>> GetFilteredNutritionistBasedOnRating(int ratingRange)
        {
            var nutritionists = await _context.Nutritionists
                .Include(n => n.Ratings)
                .ToListAsync();

            var filteredNutritionists = nutritionists
                .Where(n => n.AverageRating >= ratingRange && n.AverageRating < ratingRange + 1)
                .ToList();

            return filteredNutritionists;
        }

        public async Task<ActionResult<double>> MyRating(int UserId)
        {
            var nutritionist = await _context.Nutritionists
                .Include(n => n.Ratings) 
                .FirstOrDefaultAsync(p => p.UserId == UserId);

          
            return nutritionist.AverageRating;
        }
       







    }
}
