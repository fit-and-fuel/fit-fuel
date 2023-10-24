using fit_and_fuel.Data;
using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace fit_and_fuel.Services
{
    public class PriceService : IPrice
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PriceService(AppDbContext context, IHttpContextAccessor httpContextAccessor) {
        _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Price> Post(PriceDto priceDto)
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var nutrition = await _context.Nutritionists.Where(a => a.UserId == userId).FirstOrDefaultAsync();
            var price = new Price()
            {
                amount = priceDto.amount,
                NutritionistId = nutrition.Id,
            };
            _context.Prices.Add(price);
          await  _context.SaveChangesAsync();

            return price;

        }
    }
}
