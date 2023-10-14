using fit_and_fuel.Data;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace fit_and_fuel.Model
{
    public class ChatHub : Hub
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        //public async Task SendMessage(ChatMessage message)
        //{
        //    // Store the message in the database
        //    // ...

        //    // Broadcast the message to all connected clients
        //    await Clients.All.SendAsync("ReceiveMessage", message);
        //}
        public ChatHub(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext= dbContext;
            _httpContextAccessor= httpContextAccessor;
        }
        public async Task SendMessage( string message)
        {
            // Store the message in the database
            // ...
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var myprofile = await _dbContext.Patients
              .Where(p => p.UserId == userId)
              .Include(p => p.nutritionist)
              .FirstOrDefaultAsync();
            var message1 = new ChatMessage();
            // Set the sender's user ID
            message1.SenderId = userId;
            // Set the timestamp
            message1.Timestamp = DateTime.UtcNow;
            message1.ReceiverId = myprofile.nutritionist.UserId;
            message1.Content = message;

            // Save the message to the database
            _dbContext.ChatMessages.Add(message1);
            await _dbContext.SaveChangesAsync();
            // Broadcast the message to all connected clients

            await Clients.User(myprofile.nutritionist.UserId).SendAsync("ReceiveMessage",message);
            await Clients.User(userId).SendAsync("ReceiveMessage",  message);

        }
        public async Task SendMessageNut(string message, string toUser)
        {
            // Store the message in the database
            // ...
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var myprofile = await _dbContext.Nutritionists
              .Where(p => p.UserId == userId)
              .FirstOrDefaultAsync();
            var toUserid = await _dbContext.Patients
              .Where(p => p.Id.ToString() == toUser)
              .FirstOrDefaultAsync();
            var message1 = new ChatMessage();
            // Set the sender's user ID
            message1.SenderId = userId;
            // Set the timestamp
            message1.Timestamp = DateTime.UtcNow;
            message1.ReceiverId = toUserid.UserId;
            message1.Content = message;

            // Save the message to the database
            _dbContext.ChatMessages.Add(message1);
            await _dbContext.SaveChangesAsync();
            // Broadcast the message to all connected clients

            await Clients.User(toUserid.UserId).SendAsync("ReceiveMessage", message);
            await Clients.User(userId).SendAsync("ReceiveMessage", message);

        }
        //public async Task SendMessage(string fromUser, string message, string toUser)
        //{
        //    // Store the message in the database
        //    // ...
        //    string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        //    var message1 = new ChatMessage();
        //    // Set the sender's user ID
        //    message1.SenderId = userId;
        //    // Set the timestamp
        //    message1.Timestamp = DateTime.UtcNow;
        //    message1.ReceiverId = toUser;
        //    message1.Content = message;

        //    // Save the message to the database
        //    _dbContext.ChatMessages.Add(message1);
        //    await _dbContext.SaveChangesAsync();
        //    // Broadcast the message to all connected clients

        //    await Clients.User(toUser).SendAsync("ReceiveMessage", userId, message);
        //    await Clients.User(userId).SendAsync("ReceiveMessage", userId, message);


        //}
    }

}
