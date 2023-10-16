using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;
using fit_and_fuel.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace fit_and_fuel.Controllers
{
    public class ChatController : Controller
    {
        private readonly IChatMessage _chatMessage;
        public ChatController(IChatMessage chatMessage)
        {
            _chatMessage = chatMessage;
        }
        public async Task<IActionResult> Index()
        {
            var messages = await _chatMessage.ReceiveMessages();
            return View(messages);
        }
        public async Task<IActionResult> nutritionistChat(int id)
        {
            var messages = await _chatMessage.ReceiveMessagesNutritionist(id);
            var nutritionistModle = new NutritionistVM
            {
                Patinetid = id,
                ChatMessages = messages
            };

            return View(nutritionistModle);
        }
        [HttpPost]
        public async Task<IActionResult> SendMessage(ChatMessageDto messageDto)
        {
            await _chatMessage.SendMessage(messageDto);
            return Redirect("index");
        }
    }
}
