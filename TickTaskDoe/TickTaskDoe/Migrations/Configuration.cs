namespace TickTaskDoe.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using TickTaskDoe.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TickTaskDoe.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(TickTaskDoe.Models.ApplicationDbContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //

            AddDefaultUser(context);
        }

        void AddDefaultUser(TickTaskDoe.Models.ApplicationDbContext context)
        {
            var user = new ApplicationUser { UserName = "DefaultUser",Email= "DefaultUser@msft.com",FirstName="Default",LastName="User" };
            var um = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(context));
            um.Create(user, "Password");
        }
    }
}
