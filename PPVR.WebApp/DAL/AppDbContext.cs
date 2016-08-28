using Microsoft.AspNet.Identity.EntityFramework;
using PPVR.WebApp.Models;

namespace PPVR.WebApp.DAL
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext()
            : base("DefaultConnection", false)
        {
        }

        public static AppDbContext Create()
        {
            return new AppDbContext();
        }
    }
}