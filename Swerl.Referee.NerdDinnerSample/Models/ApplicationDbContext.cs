using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Swerl.Referee.NerdDinnerSample.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Dinner> Dinners { get; set; } 

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<ApplicationDbContext>(new DropCreateDatabaseIfModelChanges<ApplicationDbContext>());
        }
    }
}