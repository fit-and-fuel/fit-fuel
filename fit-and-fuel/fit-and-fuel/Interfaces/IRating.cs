using fit_and_fuel.DTOs;
using fit_and_fuel.Model;
using Microsoft.AspNetCore.Mvc;

namespace fit_and_fuel.Interfaces
{
    public interface IRating
    {
        Task<List<Rating>> GetRatingsForNutritionist(int nutritionistId);
        Task<Rating> AddRating(RatingDto ratingDto);
        Task<ActionResult<double>> MyRating(int UserId);
        Task<List<Nutritionist>> GetFilteredNutritionistBasedOnRating(int ratingRange);
    }

}
