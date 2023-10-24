using fit_and_fuel.Interfaces;
using fit_and_fuel.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace fit_and_fuel.Components
{
    [ViewComponent]
    public class ChatNut: ViewComponent
    {
        private readonly IChatMessage _chatMessage;
        private readonly IPatients _petients;
        public ChatNut(IChatMessage chatMessage)
        {
            _chatMessage = chatMessage;

        }
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {

            var messages = await _chatMessage.ReceiveMessagesNutritionist(id);
            var nutritionistModle = new NutritionistVM
            {
                Patinetid = id,
                ChatMessages = messages
            };

            return View(nutritionistModle);
        }

    }
}
