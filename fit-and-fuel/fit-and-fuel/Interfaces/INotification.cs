using fit_and_fuel.DTOs;
using fit_and_fuel.Model;

namespace fit_and_fuel.Interfaces
{
    public interface INotification
    {
        Task<List<Notification>> GetNotificationsForUser();
        Task SendNotification(string userId, NotificationDto notificationDto);

        Task<int> CountNot();
    }
}
