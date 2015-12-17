namespace Eugenie.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    using System.Linq;

    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;

    using Models;

    public sealed class Configuration : DbMigrationsConfiguration<EugenieDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(EugenieDbContext context)
        {
            var manager = new UserManager<User>(new UserStore<User>(new EugenieDbContext()));

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new EugenieDbContext()));

            var user = new User()
            {
                UserName = "Eugenie",
                FirstName = "Eugenie",
                LastName = "Eugenie"
            };

            manager.Create(user, "Eugenie");

            if (!roleManager.Roles.Any())
            {
                roleManager.Create(new IdentityRole { Name = "Admin" });
                roleManager.Create(new IdentityRole { Name = "Seller" });
            }

            var adminUser = manager.FindByName("Eugenie");

            manager.AddToRole(adminUser.Id, "Admin");
        }
    }
}