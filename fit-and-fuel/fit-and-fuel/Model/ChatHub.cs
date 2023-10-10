using Microsoft.AspNetCore.SignalR;

namespace fit_and_fuel.Model
{
    public class ChatHub : Hub
    {
        public async Task SendMessage(ChatMessage message)
        {
            // Store the message in the database
            // ...

            // Broadcast the message to all connected clients
            await Clients.All.SendAsync("ReceiveMessage", message);
        }
    }

}
