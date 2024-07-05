using How_To_Approved_Record_MVC_Application.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace How_To_Approved_Record_MVC_Application.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IHubContext<NotificationHub> _hubContext;

        public AdminController(ApplicationDbContext context, IHubContext<NotificationHub> hubContext)
        {
            _context = context;
            _hubContext = hubContext;
        }
        public IActionResult Index()
        {
            // Get IDs of approved notifications
            var approvedNotificationIds = _context.Notifications
                .Where(n => n.IsApproved)
                .Select(n => n.Id)
                .ToList();

            // Get students associated with approved notifications
            var approvedStudents = _context.Students
                .Where(s => approvedNotificationIds.Contains(s.Id))
                .ToList();

            return View(approvedStudents);
        }
        public IActionResult CreateStudent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(Student student)
        {
            using (var trans = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        _context.Students.Add(student);
                        await _context.SaveChangesAsync();

                        // Send notification to CEO for approval
                        var notification = new Notification
                        {
                            Message = $"New student added: {student.FirstName} {student.LastName}",
                            CreatedAt = DateTime.Now,
                            IsApproved = false // Initially not approved
                        };

                        _context.Notifications.Add(notification);
                        await _context.SaveChangesAsync();
                        await trans.CommitAsync();
                        // Notify CEO through SignalR
                        await _hubContext.Clients.All.SendAsync("ReceiveNotification", notification);

                        return RedirectToAction(nameof(Index));
                        // Redirect to home page or student list
                    }
                }
                catch (Exception)
                {

                   await trans.RollbackAsync();
                }
                
            }
           

            return View(student);

           
        }
    }

}
