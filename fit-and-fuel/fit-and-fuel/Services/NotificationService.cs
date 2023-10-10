using fit_and_fuel.Data;
using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace fit_and_fuel.Services
{
    /// <summary>
    /// Service class responsible for managing user notifications and sending notifications through SignalR.
    /// </summary>
    
    public class NotificationService : INotification
    {
        private readonly AppDbContext _dbContext;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(AppDbContext dbContext, IHubContext<NotificationHub> hubContext)
        {
            _dbContext = dbContext;
            _hubContext = hubContext;
        }

        /// <summary>
        /// Retrieves a list of notifications for a specific user, ordered by timestamp in descending order.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve notifications for.</param>
        /// <returns>A list of notifications for the user.</returns>

        public async Task<List<Notification>> GetNotificationsForUser(string userId)
        {
            var notifications = await _dbContext.Notifications
        .Where(n => n.UserId == userId)
        .OrderByDescending(n => n.Timestamp)
        .ToListAsync();

            return notifications; 
        }

        /// <summary>
        /// Sends a notification to a user and adds it to the database.
        /// </summary>
        /// <param name="userId">The ID of the user to send the notification to.</param>
        /// <param name="notificationDto">The DTO containing the notification content.</param>

        public async Task SendNotification(string userId, NotificationDto notificationDto)
        {
            var notification = new Notification
            {
                UserId = userId,
                Timestamp = DateTime.UtcNow,
                Content = notificationDto.Content
            };

            _dbContext.Notifications.Add(notification);
            await _dbContext.SaveChangesAsync();

            await _hubContext.Clients.Group(userId).SendAsync("ReceiveNotification", notification);
        }
    }
}
