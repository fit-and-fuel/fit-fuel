using fit_and_fuel.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fit_and_fuel.Components
{
	
	[ViewComponent]
	public class ChatList : ViewComponent
	{
		private readonly IChatMessage _chatMessage;
        public ChatList(IChatMessage chatMessage)
        {
			_chatMessage = chatMessage;

		}
		public async Task<IViewComponentResult> InvokeAsync()
		{
			var chat =await _chatMessage.ReceiveMessages();
			return View (chat);
		}

    }
}
