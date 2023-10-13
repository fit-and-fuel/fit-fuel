using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;
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
        [HttpPost]
        public async Task<IActionResult> SendMessage(ChatMessageDto messageDto)
        {
            await _chatMessage.SendMessage(messageDto);
            return Redirect("index");
        }
    }
}
