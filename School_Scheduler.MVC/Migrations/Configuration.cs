namespace School_Scheduler.MVC.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.Linq;
    using School_Scheduler.Models.Domain;
    using School_Scheduler.MVC.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "School_Scheduler.MVC.Models.ApplicationDbContext";
        }

        protected override void Seed(ApplicationDbContext db)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.

            if (Debugger.IsAttached == false)
            {
                // Uncomment line below for debugging this Seed() method
                // System.Diagnostics.Debugger.Launch();
            }

            Instructor everett = new Instructor { Name = "Everett" };
            Instructor alvi = new Instructor { Name = "Alvi" };

            ClassRoom room200 = new ClassRoom { Name = "Some Room1", RoomNumber = 200 };
            ClassRoom room104 = new ClassRoom { Name = "Some Room2", RoomNumber = 104 };
            ClassRoom room126 = new ClassRoom { Name = "Some Room3", RoomNumber = 126 };

            SchoolProgram softwareDevelopment = new SchoolProgram
            {
                Name = "Software Development",
                Instructors = new List<Instructor> { everett, alvi }
            };

            DateTime startDate = DateTime.Now.AddDays(5);
            Course reactFrontToBack = new Course
            {
                Name = "React Front to Back",
                SchoolProgram = softwareDevelopment,
                ClassStartTime = TimeSpan.FromHours(8.45),
                ClassEndTime = TimeSpan.FromHours(3.15),
                StartDate = startDate,
                EndDate = startDate.AddYears(1),
                Instructor = everett,
                Room = room200,
            };

            db.ClassRooms.AddOrUpdate(cr => cr.RoomNumber, room200, room126, room104);
            db.SchoolPrograms.AddOrUpdate(sp => sp.Name, softwareDevelopment);
            db.Instructors.AddOrUpdate(i => i.Name, everett, alvi);
            db.Courses.AddOrUpdate(c => c.Name, reactFrontToBack);

            db.SaveChanges();
        }
    }
}
