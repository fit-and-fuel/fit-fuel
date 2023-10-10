using fit_and_fuel.Data;
using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;

using fit_and_fuel.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
//using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;

namespace fit_and_fuel.Services
{
    public class ChatMessageService : IChatMessage
    {
        private readonly AppDbContext _dbContext;
        private readonly IHubContext<ChatHub> _hubContext;

        public ChatMessageService(AppDbContext dbContext, IHubContext<ChatHub> hubContext)
        {
            _dbContext = dbContext;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Retrieves a list of chat messages received by a specific user.
        /// </summary>
        /// <param name="Id">The ID of the user receiving the messages.</param>
        /// <returns>A list of chat messages received by the user.</returns>

        public async Task<List<ChatMessage>> ReceiveMessages(string Id)
        {
           
            var messages =await _dbContext.ChatMessages
               .Where(msg => msg.ReceiverId == Id)
               .OrderBy(msg => msg.Timestamp)
               .ToListAsync();
            return messages;
        }

        /// <summary>
        /// Sends a chat message from a user to another user.
        /// </summary>
        /// <param name="userId">The ID of the sender user.</param>
        /// <param name="messageDto">The details of the message to send.</param>

        public async Task SendMessage(string userId, ChatMessageDto messageDto)
        {
            var message = new ChatMessage();
            // Set the sender's user ID
            message.SenderId = userId;
            // Set the timestamp
            message.Timestamp = DateTime.UtcNow;
            message.ReceiverId = messageDto.ReceiverId;
            message.Content = messageDto.Content;

            // Save the message to the database
            _dbContext.ChatMessages.Add(message);
            await _dbContext.SaveChangesAsync();

            // Broadcast the message to connected clients
            await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
        }
    }
}
