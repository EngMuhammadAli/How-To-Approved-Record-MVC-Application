using How_To_Approved_Record_MVC_Application.Models;
using Microsoft.EntityFrameworkCore;

namespace How_To_Approved_Record_MVC_Application
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
                
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Notification> Notifications { get; set; }
    }
}
