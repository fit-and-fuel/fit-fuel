using fit_and_fuel.Data;
using fit_and_fuel.DTOs;
using fit_and_fuel.Interfaces;
using fit_and_fuel.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace fit_and_fuel.Services
{
    /// <summary>
    /// Service class responsible for managing user notifications and sending notifications through SignalR.
    /// </summary>
    
    public class NotificationService : INotification
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly AppDbContext _dbContext;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(AppDbContext dbContext, IHubContext<NotificationHub> hubContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _hubContext = hubContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<int> CountNot()
        {
       string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var notifications = await _dbContext.Notifications
        .Where(n => n.UserId == userId)
        .OrderByDescending(n => n.Timestamp)
        .ToListAsync();
            var CountNot= notifications.Where(n => n.ReadNotfication == false).Count();
            return CountNot;
        }

        /// <summary>
        /// Retrieves a list of notifications for a specific user, ordered by timestamp in descending order.
        /// </summary>
        /// <param name="userId">The ID of the user to retrieve notifications for.</param>
        /// <returns>A list of notifications for the user.</returns>

        public async Task<List<Notification>> GetNotificationsForUser()
        {
            string userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var notifications = await _dbContext.Notifications
        .Where(n => n.UserId == userId)
        .OrderByDescending(n => n.Timestamp)
        .ToListAsync();
            foreach(var read in notifications)
            {
                read.ReadNotfication = true;
            }
            await _dbContext.SaveChangesAsync();

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
