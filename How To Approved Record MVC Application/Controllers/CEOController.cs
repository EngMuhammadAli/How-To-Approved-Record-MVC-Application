using How_To_Approved_Record_MVC_Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace How_To_Approved_Record_MVC_Application.Controllers
{
    public class CEOController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public CEOController(ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }

        public IActionResult Notifications()
        {
            var notifications = _context.Notifications.ToList();
            return View(notifications);
        }

        public async Task<IActionResult> Approve(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                notification.IsApproved = true;
                await _context.SaveChangesAsync();

                // Notify admin that student is approved
                var studentNotification = new Notification
                {
                    Message = $"Student {notification.Message} approved by CEO",
                    CreatedAt = DateTime.Now,
                    IsApproved = true
                };

                await _hubContext.Clients.All.SendAsync("ReceiveNotification", studentNotification);
            }

            return RedirectToAction("Notifications");
        }

        public async Task<IActionResult> Reject(int id)
        {
            var notification = await _context.Notifications.FindAsync(id);
            if (notification != null)
            {
                // Delete or mark as rejected
                _context.Notifications.Remove(notification);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("Notifications");
        }
    }

}
