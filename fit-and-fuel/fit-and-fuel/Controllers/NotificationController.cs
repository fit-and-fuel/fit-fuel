using fit_and_fuel.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace fit_and_fuel.Controllers
{
    public class NotificationController : Controller
    {
        private readonly INotification _notification;

        public NotificationController(INotification notification)
        {
            _notification = notification;
        }
        public async Task<IActionResult> Index()
        {
            var notfication = await _notification.GetNotificationsForUser();
            return View(notfication);
        }
    }
}
