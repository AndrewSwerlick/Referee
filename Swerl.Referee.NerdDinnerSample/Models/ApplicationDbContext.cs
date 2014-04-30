using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
            Configuration.LazyLoadingEnabled = true;
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
            var roles = new[] {"Admin", "Host"}.Select(r=> new IdentityRole(r)).ToList();
            foreach (var identityRole in roles)
            {
                context.Roles.Add(identityRole);
            }

            context.SaveChanges();

            context.ActivityPermissions.AddRange(new List<ActivityPermission>
            {
                new ActivityPermission{Name = "Delete", Roles = new IdentityRole[]{roles.First(), }}
            });
            context.SaveChanges();
        }
    }
}