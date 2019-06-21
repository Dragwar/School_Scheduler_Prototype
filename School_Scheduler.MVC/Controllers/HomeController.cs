using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using School_Scheduler.MVC.Filters;
using School_Scheduler.MVC.Helpers;
using School_Scheduler.MVC.Models;
using School_Scheduler.MVC.Models.Domain;
using School_Scheduler.MVC.Models.ViewModels;
using static School_Scheduler.MVC.Helpers.UserIdentityExtensions;

namespace School_Scheduler.MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(CalenderDataViewModel model)
        {
            string currentUserId = User.Identity.GetUserId();
            Discriminator discriminator = User.Identity.GetUserDiscriminator();
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser foundCurrentUser = db.Users.FirstOrDefault(user => user.Id == currentUserId);

            if (discriminator == Discriminator.Instructor)
            {
                if (((Instructor)foundCurrentUser).SchoolProgram == null)
                {
                    ModelState.AddModelError("key", "You don't have a SchoolProgram to display");
                    return View("Error");
                }

                SchoolProgram schoolProgram = ((Instructor)foundCurrentUser).SchoolProgram;
                CalenderDataViewModel viewModel = new CalenderDataViewModel(schoolProgram);
                if (model != null)
                {
                    viewModel.Today = model.Today;
                }
                return View(viewModel);
            }
            else if (discriminator == Discriminator.Student)
            {
                if (((Student)foundCurrentUser).SchoolProgram == null)
                {
                    ModelState.AddModelError("key", "You don't have a SchoolProgram to display");
                    return View("Error");
                }

                Course currentCourse = ((Student)foundCurrentUser).CurrentCourse;
                IndexForStudentViewModel viewModel = new IndexForStudentViewModel
                {
                    CurrentCourse = new CourseViewModel
                    {
                        Id = currentCourse.Id,
                        Name = currentCourse.Name,
                        StartDate = currentCourse.StartDate,
                        EndDate = currentCourse.EndDate,
                        SchoolProgram = new SchoolProgramViewModel
                        {
                            Id = currentCourse.SchoolProgramId,
                            Name = currentCourse.SchoolProgram.Name
                        },
                        EnrolledStudents = new List<StudentViewModel>
                        {
                            new StudentViewModel
                            {
                                Id = foundCurrentUser.Id,
                                Name = foundCurrentUser.Name
                            }
                        }
                    }
                };
                if (model != null)
                {
                    viewModel.Today = model.Today;
                }
                return View("IndexForStudent", viewModel);
            }
            ModelState.AddModelError("Invalid User", "Hello there");
            return View("Error");
        }


        public ActionResult CoursesByDate(DateTime date)
        {
            string currentUserId = User.Identity.GetUserId();
            Discriminator discriminator = User.Identity.GetUserDiscriminator();
            ApplicationDbContext db = new ApplicationDbContext();
            ApplicationUser foundCurrentUser = db.Users.FirstOrDefault(user => user.Id == currentUserId);

            if (discriminator == Discriminator.Instructor)
            {
                if (((Instructor)foundCurrentUser).SchoolProgram == null)
                {
                    ModelState.AddModelError("key", "You don't have a SchoolProgram to display");
                    return View("Error");
                }

                var model = new CoursesByDateViewModel();
                SchoolProgram schoolProgram = ((Instructor)foundCurrentUser).SchoolProgram;
                var courses = db.Courses.Where(p => p.InstructorId == currentUserId).ToList().Where(r => date.InRange(r.StartDate, r.EndDate.AddDays(1))).Select(r => new CourseViewModel
                {
                    ClassEndTime = r.ClassEndTime,
                    ClassRoomId = r.ClassRoomId,
                    ClassRoom = new ClassRoomViewModel
                    {
                        Name = r.ClassRoom.Name,
                        RoomNumber = r.ClassRoom.RoomNumber,
                    },
                    ClassStartTime = r.ClassStartTime,
                    CourseNumber = r.CourseNumber,
                    SchoolProgramId = r.SchoolProgramId,
                    SchoolProgram = new SchoolProgramViewModel
                    {
                        Name = r.SchoolProgram.Name,
                    },
                    Instructor = new InstructorViewModel
                    {
                        Name = r.Instructor.Name,
                        InstructorNumber = r.Instructor.InstructorNumber
                    },
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    Name = r.Name,
                    InstructorId = r.InstructorId
                }).ToList();
                if (courses != null)
                {
                    model.Courses = courses;
                }
                return View(model);
            }
            else if (discriminator == Discriminator.Student)
            {
                if (((Student)foundCurrentUser).SchoolProgram == null)
                {
                    ModelState.AddModelError("key", "You don't have a SchoolProgram to display");
                    return View("Error");
                }

                var model = new CoursesByDateViewModel();
                SchoolProgram schoolProgram = ((Student)foundCurrentUser).SchoolProgram;
                var courses = db.Courses.Where(p => p.EnrolledStudents.Any(q => q.Id == currentUserId)).ToList().Where(r => date.InRange(r.StartDate, r.EndDate.AddDays(1))).Select(r => new CourseViewModel
                {
                    ClassEndTime = r.ClassEndTime,
                    ClassRoomId = r.ClassRoomId,
                    ClassRoom = new ClassRoomViewModel
                    {
                        Name = r.ClassRoom.Name,
                        RoomNumber = r.ClassRoom.RoomNumber,
                    },
                    ClassStartTime = r.ClassStartTime,
                    CourseNumber = r.CourseNumber,
                    SchoolProgramId = r.SchoolProgramId,
                    SchoolProgram = new SchoolProgramViewModel
                    {
                        Name = r.SchoolProgram.Name,
                    },
                    Instructor = new InstructorViewModel
                    {
                        Name = r.Instructor.Name,
                        InstructorNumber = r.Instructor.InstructorNumber
                    },
                    StartDate = r.StartDate,
                    EndDate = r.EndDate,
                    Name = r.Name,
                    InstructorId = r.InstructorId,
                    EnrolledStudents = r.EnrolledStudents.Select(s => new StudentViewModel(s))
                }).ToList();
                if (courses != null)
                {
                    model.Courses = courses;
                }
                return View(model);
            }
            

            ModelState.AddModelError("Invalid User", "Hello there");
            return View("Error");
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View("Error");
        }

        
        [EnsureDiscriminatorClaim(Discriminator.Instructor)]
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}