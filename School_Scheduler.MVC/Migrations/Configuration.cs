namespace School_Scheduler.MVC.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using System.Diagnostics;
    using System.Linq;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using School_Scheduler.MVC.Helpers;
    using School_Scheduler.MVC.Models;
    using School_Scheduler.MVC.Models.Domain;

    internal sealed class Configuration : DbMigrationsConfiguration<ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            ContextKey = "School_Scheduler.MVC.Models.ApplicationDbContext";
        }

        private const string password = "Password-1";
        private string Email(string name) => name.Trim().ToLower() + "@gmail.com";

        protected override void Seed(ApplicationDbContext db)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.


            if (!Debugger.IsAttached)
            {
                // Uncomment line below for debugging this Seed() method
                //System.Diagnostics.Debugger.Launch();
            }

            ApplicationUserManager userManager = new ApplicationUserManager(new UserStore<ApplicationUser>(db));

            List<Student> initialStudents = new List<Student>();
            for (int i = 1; i <= 5; i++)
            {
                string studentEmail = Email(nameof(Student) + i);
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
                UserName = Email("Everett"),
                Email = Email("Everett"),
                EmailConfirmed = true,
            };
            Instructor alvi = new Instructor
            {
                Name = "Alvi",
                //InstructorNumber = ,
                UserName = Email("Alvi"),
                Email = Email("Alvi"),
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
                EndDate = startDate.AddDays(5),
                Instructor = everett,
                InstructorId = everett.Id,
                ClassRoom = room200,
                ClassRoomId = room200.Id,
                //CourseNumber = ,
                //EnrolledStudents = new List<Student>(initialStudents),
            };

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



            // OLD WAY
            #region Second Batch
            //int numberOfExistingStudents = db.Users.Where(user => user is Student).Count() + 1;
            //List<Student> secondListOfStudents = new List<Student>();
            //for (int i = numberOfExistingStudents; i < numberOfExistingStudents + 10; i++)
            //{
            //    string email = Email($"student{i}");
            //    secondListOfStudents.Add(new Student { Name = $"Student Name {i}", UserName = email, Email = email, EmailConfirmed = true, });
            //}

            //int numberOfExistingInstructors = db.Users.Where(user => user is Instructor).Count() + 1;
            //List<Instructor> secondListOfInstructors = new List<Instructor>();
            //for (int i = numberOfExistingInstructors; i < numberOfExistingInstructors + 3; i++)
            //{
            //    string email = Email($"instructor{i}");
            //    secondListOfInstructors.Add(new Instructor { Name = $"Instructor Name {i}", UserName = email, Email = email, EmailConfirmed = true, });
            //}

            //int numberOfExistingCourses = db.Courses.Count() + 1;
            //List<Course> secondListOfCourses = new List<Course>();
            //for (int i = numberOfExistingCourses; i < numberOfExistingCourses + 3; i++)
            //{
            //    DateTime startingDate;
            //    if (i % 2 == 0)
            //    {
            //        startingDate = DateTime.Now.AddMonths(i - 1);
            //    }
            //    else
            //    {
            //        startingDate = DateTime.Now.AddMonths(i + 1);
            //    }
            //    DateTime endingDate = startingDate.AddMonths(i + 2);
            //    secondListOfCourses.Add(new Course
            //    {
            //        Name = $"Course Name {i}",
            //        StartDate = startingDate,
            //        EndDate = endingDate,
            //        ClassStartTime = new TimeSpan(8, 45, 0),
            //        ClassEndTime = new TimeSpan(15, 15, 0)
            //    });
            //}
            #endregion

            // NEWER WAY
            //GenerateNextEmptyBatchOfStudentsInstructorsCourses(
            //    db: db,
            //    listOfStudents: out List<Student> secondListOfStudents,
            //    listOfInstructors: out List<Instructor> secondListOfInstructors,
            //    listOfCourses: out List<Course> secondListOfCourses);

            //SchoolProgram autoGeneratedProgram = GenerateSchoolProgram(
            //    db: db,
            //    userManager: userManager,
            //    schoolProgramName: "CAD Design",
            //    classRooms: new List<ClassRoom> { room126, room104 },
            //    toBeEnrolledStudents: secondListOfStudents,
            //    instructors: secondListOfInstructors,
            //    courses: secondListOfCourses.ToArray());

            GereateNextBatchOfAll(db, userManager, "CAD Design", new List<ClassRoom> { room104, room126, room200 });

            db.SaveChanges();
        }

        private void GereateNextBatchOfAll(
            ApplicationDbContext db,
            ApplicationUserManager userManager,
            string schoolProgramName,
            List<ClassRoom> classRooms,
            int generateNumberOfStudents = 10,
            int generateNumberOfInstructors = 3,
            int generateNumberOfCourses = 3)
        {
            if (db == null)
            {
                throw new ArgumentNullException(nameof(db));
            }
            if (userManager == null)
            {
                throw new ArgumentNullException(nameof(userManager));
            }
            if (string.IsNullOrWhiteSpace(schoolProgramName))
            {
                throw new ArgumentException("A name is required", nameof(schoolProgramName));
            }
            else if (db.SchoolPrograms.Any(sp => sp.Name.ToLower().Trim() == schoolProgramName.ToLower().Trim()))
            {
                throw new ArgumentException("The given name is already taken", nameof(schoolProgramName));
            }
            if (classRooms == null)
            {
                throw new ArgumentNullException(nameof(classRooms));
            }
            else if (!classRooms.Any())
            {
                throw new ArgumentException("You must provide at least one ClassRoom");
            }

            GenerateNextEmptyBatchOfStudentsInstructorsCourses(
                db: db,
                listOfStudents: out List<Student> secondListOfStudents,
                listOfInstructors: out List<Instructor> secondListOfInstructors,
                listOfCourses: out List<Course> secondListOfCourses,
                generateNumberOfStudents: generateNumberOfStudents,
                generateNumberOfInstructors: generateNumberOfInstructors,
                generateNumberOfCourses: generateNumberOfCourses);

            SchoolProgram autoGeneratedProgram = GenerateSchoolProgram(
                db: db,
                userManager: userManager,
                schoolProgramName: schoolProgramName,
                classRooms: classRooms,
                toBeEnrolledStudents: secondListOfStudents,
                instructors: secondListOfInstructors,
                courses: secondListOfCourses.ToArray());
        }

        private void GenerateNextEmptyBatchOfStudentsInstructorsCourses(
            ApplicationDbContext db,
            out List<Student> listOfStudents,
            out List<Instructor> listOfInstructors,
            out List<Course> listOfCourses,
            int generateNumberOfStudents = 10,
            int generateNumberOfInstructors = 3,
            int generateNumberOfCourses = 3)
        {
            if (generateNumberOfCourses <= 0 || generateNumberOfInstructors <= 0 || generateNumberOfStudents <= 0)
            {
                throw new ArgumentException("generateNumberOf[Courses/Instructors/Students] can't be less than or equal to zero");
            }

            int numberOfExistingStudents = db.Users.Where(user => user is Student).Count() + 1;
            List<Student> myListOfStudents = new List<Student>();
            for (int i = numberOfExistingStudents; i < numberOfExistingStudents + generateNumberOfStudents; i++)
            {
                string email = Email($"student{i}");
                myListOfStudents.Add(new Student { Name = $"Student Name {i}", UserName = email, Email = email, EmailConfirmed = true, });
            }

            int numberOfExistingInstructors = db.Users.Where(user => user is Instructor).Count() + 1;
            List<Instructor> myListOfInstructors = new List<Instructor>();
            for (int i = numberOfExistingInstructors; i < numberOfExistingInstructors + generateNumberOfInstructors; i++)
            {
                string email = Email($"instructor{i}");
                myListOfInstructors.Add(new Instructor { Name = $"Instructor Name {i}", UserName = email, Email = email, EmailConfirmed = true, });
            }

            int numberOfExistingCourses = db.Courses.Count() + 1;
            List<Course> myListOfCourses = new List<Course>();
            for (int i = numberOfExistingCourses; i < numberOfExistingCourses + generateNumberOfCourses; i++)
            {
                DateTime startingDate = DateTime.Now.AddMonths(i + 1);
                DateTime endingDate = startingDate.AddMonths(i + 2);
                myListOfCourses.Add(new Course
                {
                    Name = $"Course Name {i}",
                    StartDate = startingDate,
                    EndDate = endingDate,
                    ClassStartTime = new TimeSpan(8, 45, 0),
                    ClassEndTime = new TimeSpan(15, 15, 0)
                });
            }
            listOfStudents = myListOfStudents;
            listOfInstructors = myListOfInstructors;
            listOfCourses = myListOfCourses;
        }
        private SchoolProgram GenerateSchoolProgram(
            ApplicationDbContext db,
            ApplicationUserManager userManager,
            string schoolProgramName,
            List<ClassRoom> classRooms,
            List<Student> toBeEnrolledStudents,
            List<Instructor> instructors,
            params Course[] courses)
        {
            #region Null/Invalid Parameter Checks
            if (string.IsNullOrWhiteSpace(schoolProgramName))
            {
                throw new ArgumentException("You must provide a name for the school program", nameof(schoolProgramName));
            }
            if (classRooms == null)
            {
                throw new ArgumentNullException(nameof(classRooms));
            }
            if (toBeEnrolledStudents == null)
            {
                throw new ArgumentNullException(nameof(toBeEnrolledStudents));
            }
            if (instructors == null)
            {
                throw new ArgumentNullException(nameof(instructors));
            }
            if (courses == null)
            {
                throw new ArgumentNullException(nameof(courses));
            }
            if (!courses.Any())
            {
                throw new ArgumentException("You must provide at least one course", nameof(courses));
            }
            if (!classRooms.Any())
            {
                throw new ArgumentException("You must provide at least one class room", nameof(classRooms));
            }
            if (!toBeEnrolledStudents.Any())
            {
                throw new ArgumentException("You must provide at least one student", nameof(toBeEnrolledStudents));
            }
            if (!instructors.Any())
            {
                throw new ArgumentException("You must provide at least one instructor", nameof(instructors));
            }
            #endregion

            SchoolProgram program = new SchoolProgram { Name = schoolProgramName };

            program.Instructors.AddRange(instructors);
            program.EnrolledStudents.AddRange(toBeEnrolledStudents);

            foreach (Course course in courses)
            {
                ClassRoom randomRoom = classRooms.PickRandom();
                Instructor randomInstructor = instructors.PickRandom();
                course.SchoolProgram = program;
                course.SchoolProgramId = program.Id;
                course.Instructor = randomInstructor;
                course.InstructorId = randomInstructor.Id;
                course.ClassRoom = randomRoom;
                course.ClassRoomId = randomRoom.Id;
                randomInstructor.Courses.Add(course);
                program.Courses.Add(course);
                randomRoom.Courses.Add(course);
            }

            db.ClassRooms.AddOrUpdate(cr => cr.RoomNumber, classRooms.ToArray());
            db.SchoolPrograms.AddOrUpdate(sp => sp.Name, program);
            db.Courses.AddOrUpdate(c => c.Name, courses.ToArray());

            Course oldestCourse = program.Courses.OrderBy(course => course.StartDate).First();
            oldestCourse.EnrolledStudents.AddRange(toBeEnrolledStudents);
            db.Courses.AddOrUpdate(c => c.Name, courses);

            instructors.ForEach(instructor =>
            {
                if (!db.Users.Any(user => user.UserName == instructor.UserName))
                {
                    IdentityResult instructorResult = userManager.CreateAsync(instructor, password).Result;
                }
                else
                {
                    db.Users.AddOrUpdate(i => i.UserName, instructor);
                }
            });

            toBeEnrolledStudents.ForEach(student =>
            {
                if (!db.Users.Any(user => user.UserName == student.UserName))
                {
                    IdentityResult studentResult = userManager.CreateAsync(student, password).Result;
                }
                else
                {
                    db.Users.AddOrUpdate(s => s.UserName, student);
                }
            });

            db.SaveChanges();
            return program;
        }
    }
}
