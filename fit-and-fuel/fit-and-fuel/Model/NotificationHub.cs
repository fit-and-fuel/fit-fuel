using Microsoft.AspNetCore.SignalR;
using System.Text.RegularExpressions;

namespace fit_and_fuel.Model
{
    public class NotificationHub : Hub
    {
        public async Task JoinUserGroup(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
        }

        public async Task LeaveUserGroup(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
        }

        public async Task SendNotification(string userId, Notification notification)
        {
            await Clients.Group(userId).SendAsync("ReceiveNotification", notification);
        }
    }
}
