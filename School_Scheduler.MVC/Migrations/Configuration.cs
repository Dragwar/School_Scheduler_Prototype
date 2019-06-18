namespace School_Scheduler.MVC.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.Linq;
    using System.Reflection.Emit;
    using System.Security;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using School_Scheduler.MVC.Models;
    using School_Scheduler.MVC.Models.Domain;

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

            if (!Debugger.IsAttached)
            {
                // Uncomment line below for debugging this Seed() method
                System.Diagnostics.Debugger.Launch();
            }

            ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));

            string email(string name) => name.Trim().ToLower() + "gmail.com";
            const string password = "Password-1";

            List<Student> initialStudents = new List<Student>();
            for (int i = 1; i <= 5; i++)
            {
                string studentEmail = email(nameof(Student) + i);
                Student student = new Student
                {
                    Name = $"Student Name {i}",
                    //StudentNumber = ,
                    UserName = studentEmail,
                    Email = studentEmail,
                    EmailConfirmed = true,
                };
                initialStudents.Add(student);
            }


            Instructor everett = new Instructor
            {
                Name = "Everett",
                //InstructorNumber = ,
                UserName = email("Everett"),
                Email = email("Everett"),
                EmailConfirmed = true,
            };
            Instructor alvi = new Instructor
            {
                Name = "Alvi",
                //InstructorNumber = ,
                UserName = email("Alvi"),
                Email = email("Alvi"),
                EmailConfirmed = true,
            };

            ClassRoom room200 = new ClassRoom { Name = "Some Room1", RoomNumber = 200 };
            ClassRoom room104 = new ClassRoom { Name = "Some Room2", RoomNumber = 104 };
            ClassRoom room126 = new ClassRoom { Name = "Some Room3", RoomNumber = 126 };

            SchoolProgram softwareDevelopment = new SchoolProgram
            {
                Name = "Software Development",
                //EnrolledStudents = new List<Student>(initialStudents),
                //Instructors = new List<Instructor> { everett, alvi },
            };

            DateTime startDate = DateTime.Now.AddDays(5);
            Course reactFrontToBack = new Course
            {
                Name = "React Front to Back",
                SchoolProgram = softwareDevelopment,
                SchoolProgramId = softwareDevelopment.Id,
                ClassStartTime = new TimeSpan(8, 45, 0),
                ClassEndTime = new TimeSpan(15, 15, 0),
                StartDate = startDate,
                EndDate = startDate.AddYears(1),
                Instructor = everett,
                InstructorId = everett.Id,
                ClassRoom = room200,
                ClassRoomId = room200.Id,
                //CourseNumber = ,
                //EnrolledStudents = new List<Student>(initialStudents),
            };
            //softwareDevelopment.Courses.Add(reactFrontToBack);
            //room200.Courses.Add(reactFrontToBack);
            //everett.Courses.Add(reactFrontToBack);
            //alvi.Courses.Add(reactFrontToBack);
            //initialStudents.ForEach(student =>
            //{
            //    student.CurrentCourse = reactFrontToBack;
            //    student.CurrentCourseId = reactFrontToBack.Id;
            //});
            //everett.SchoolProgram = softwareDevelopment;
            //everett.SchoolProgramId = softwareDevelopment.Id;
            //alvi.SchoolProgram = softwareDevelopment;
            //alvi.SchoolProgramId = softwareDevelopment.Id;

            db.ClassRooms.AddOrUpdate(cr => cr.RoomNumber, room200, room126, room104);
            db.SchoolPrograms.AddOrUpdate(sp => sp.Name, softwareDevelopment);
            db.Courses.AddOrUpdate(c => c.Name, reactFrontToBack);

            //everett.SchoolProgram = softwareDevelopment;
            //everett.SchoolProgramId = softwareDevelopment.Id;
            db.SchoolPrograms.AddOrUpdate(sp => sp.Name, softwareDevelopment);
            softwareDevelopment.Instructors.Add(everett);
            if (!db.Users.Any(user => user.UserName == everett.UserName))
            {
                IdentityResult everettResult = userManager.CreateAsync(everett, password).Result;
            }
            else
            {
                db.Users.AddOrUpdate(user => user.UserName, everett);
            }

            //alvi.SchoolProgram = softwareDevelopment;
            //alvi.SchoolProgramId = softwareDevelopment.Id;
            db.SchoolPrograms.AddOrUpdate(sp => sp.Name, softwareDevelopment);
            softwareDevelopment.Instructors.Add(alvi);
            if (!db.Users.Any(user => user.UserName == alvi.UserName))
            {
                IdentityResult alviResult = userManager.CreateAsync(alvi, password).Result;
            }
            else
            {
                db.Users.AddOrUpdate(user => user.UserName, alvi);
            }

            initialStudents.ForEach(student =>
            {
                //student.SchoolProgram = softwareDevelopment;
                //student.SchoolProgramId = softwareDevelopment.Id;
                //student.CurrentCourse = reactFrontToBack;
                //student.CurrentCourseId = reactFrontToBack.Id;
                softwareDevelopment.EnrolledStudents.Add(student);
                reactFrontToBack.EnrolledStudents.Add(student);
                db.Courses.AddOrUpdate(c => c.Name, reactFrontToBack);
                db.SchoolPrograms.AddOrUpdate(sp => sp.Name, softwareDevelopment);

                if (!db.Users.Any(user => user.UserName == student.UserName))
                {
                    IdentityResult studentResult = userManager.CreateAsync(student, password).Result;
                }
                else
                {
                    db.Users.AddOrUpdate(i => i.UserName, student);
                }
            });


            db.Users.AddOrUpdate(i => i.UserName, everett, alvi);
            db.Users.AddOrUpdate(i => i.UserName, initialStudents.ToArray());


            db.SaveChanges();
        }
    }
}
