using fit_and_fuel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace fit_and_fuel.Components
{
    [ViewComponent]
    public class MyPatient : ViewComponent
    {
        private readonly INutritionists _nutritionists;
        public MyPatient(INutritionists nutritionists)
        {
            _nutritionists = nutritionists;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var av = await _nutritionists.GetMyProfile();
            return View(av);

        }
    }
}
