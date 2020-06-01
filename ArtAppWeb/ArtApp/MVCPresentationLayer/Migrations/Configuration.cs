namespace MVCPresentationLayer.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using MVCPresentationLayer.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<MVCPresentationLayer.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "MVCPresentationLayer.Models.ApplicationDbContext";
        }

        protected override void Seed(MVCPresentationLayer.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            // Code from step 6
            const string admin = "admin@company.com";
            const string adminPassword = "P@ssw0rd";

            LogicLayer.UserManager userMgr = new LogicLayer.UserManager();
            var roles = userMgr.RetrieveUserRoles();
            foreach (var role in roles)
            {
                context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = role });
            }

            if (!roles.Contains("Administrator"))
            {
                context.Roles.AddOrUpdate(r => r.Name, new IdentityRole() { Name = "Admin" });
            }

            if (!context.Users.Any(u => u.UserName == admin))
            {
                var user = new ApplicationUser()
                {
                    UserName = admin,
                    Email = admin,
                    DisplayName = "Company Admin"
                };

                IdentityResult result = userManager.Create(user, adminPassword);
                context.SaveChanges(); // updates DB

                // Adds Admin role to administrator
                if (result.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Admin");
                    context.SaveChanges();
                }

            }
        }
    }
}
