using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Swerl.Referee.NerdDinnerSample.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Dinner> Dinners { get; set; }
        public DbSet<ActivityPermission> ActivityPermissions { get; set; } 

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<ApplicationDbContext>(new DropAndAddData());
        }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ActivityPermission>()                
                .HasKey(a=> a.Name)
                .HasMany(a => a.Roles)
                .WithMany()
                .Map(c =>
                {
                    c.MapLeftKey("ActivityPermission_Name");
                    c.MapRightKey("IdentityRole_Id");
                    c.ToTable("ActivityPermissionIdentityRoles");
                });

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });
        } 
    }

    public class DropAndAddData : DropCreateDatabaseIfModelChanges<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            context.ActivityPermissions.AddRange(new List<ActivityPermission>
            {
                new ActivityPermission{Name = "Delete", Roles = new IdentityRole[]{new IdentityRole("Admin"), }}
            });
            context.SaveChanges();
        }
    }
}