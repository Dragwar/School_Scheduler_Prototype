using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using School_Scheduler.MVC.Filters;
using School_Scheduler.MVC.Helpers;
using School_Scheduler.MVC.Models;
using School_Scheduler.MVC.Models.Domain;
using School_Scheduler.MVC.Models.ViewModels;

namespace School_Scheduler.MVC.Controllers
{
    public class CourseController : Controller
    {
        private ApplicationDbContext DbContext { get; } = new ApplicationDbContext();

        // GET: Course
        [HttpGet]
        [EnsureDiscriminatorClaim(Discriminator.Instructor, Discriminator.Student)]
        public ActionResult Index()
        {
            string userId = User.Identity.GetUserId();
            Discriminator userDiscriminator = User.Identity.GetUserDiscriminator();
            ApplicationUser foundUser = DbContext.Users.FirstOrDefault(user => userId == user.Id);

            // this check isn't really needed because [EnsureDiscriminatorClaim]
            if (foundUser == null)
            {
                ModelState.AddModelError("", "You must be logged in. (If you already are contact an admin for assistance)");
                return View("Error");
            }

            List<SchoolProgram> usersSchoolPrograms;
            switch (foundUser)
            {
                case Instructor _: usersSchoolPrograms = DbContext.SchoolPrograms.Where(sp => sp.Instructors.Any(i => i.Id == userId)).ToList(); break;
                case Student _: usersSchoolPrograms = DbContext.SchoolPrograms.Where(sp => sp.EnrolledStudents.Any(i => i.Id == userId)).ToList(); break;

                // these checks aren't really needed because [EnsureDiscriminatorClaim]
                case ApplicationUser _: ModelState.AddModelError("", "You must be a Student or Instructor for viewing courses"); return View("Error");
                default: ModelState.AddModelError("", "You must be logged in. (If you already are contact an admin for assistance)"); return View("Error");
            }

            ViewBag.SchoolPrograms = usersSchoolPrograms;

            return View();
        }

        [NonAction]
        private (SelectList SchoolPrograms, SelectList ClassRooms) BuildCreateViewModel(CreateCourseViewModel existingViewModel = null)
        {
            string userId = User.Identity.GetUserId();
            List<SchoolProgram> usersSchoolPrograms = DbContext.SchoolPrograms.Where(sp => sp.Instructors.Any(i => i.Id == userId)).ToList();
            List<ClassRoom> classRooms = DbContext.ClassRooms.ToList();

            if (usersSchoolPrograms == null || !usersSchoolPrograms.Any())
            {
                if (classRooms == null || !classRooms.Any())
                {
                    return (null, null);
                }
                return (null, new SelectList(classRooms, nameof(ClassRoom.Id), nameof(ClassRoom.Name), existingViewModel?.ClassRoomId));
            }
            if (classRooms == null || !classRooms.Any())
            {
                if (usersSchoolPrograms == null || !usersSchoolPrograms.Any())
                {
                    return (null, null);
                }
                return (new SelectList(usersSchoolPrograms, nameof(SchoolProgram.Id), nameof(SchoolProgram.Name), existingViewModel?.SchoolProgramId), null);
            }
            return (
                SchoolPrograms: new SelectList(usersSchoolPrograms, nameof(SchoolProgram.Id), nameof(SchoolProgram.Name), existingViewModel?.SchoolProgramId),
                ClassRooms: new SelectList(classRooms, nameof(ClassRoom.Id), nameof(ClassRoom.Name), existingViewModel?.ClassRoomId));
        }

        [HttpGet]
        [EnsureDiscriminatorClaim(Discriminator.Instructor)]
        public ActionResult Create()
        {
            (SelectList schoolPrograms, SelectList classRooms) = BuildCreateViewModel();
            if (classRooms == null || !classRooms.Any())
            {
                ModelState.AddModelError("", "No Class Rooms were found");
                return View("Error");
            }

            if (schoolPrograms == null || !schoolPrograms.Any())
            {
                ModelState.AddModelError("", "No School Programs were found (you currently don't belong to any school program)");
                return View("Error");
            }

            CreateCourseViewModel viewModel = new CreateCourseViewModel
            {
                ClassRooms = classRooms,
                SchoolPrograms = schoolPrograms,
                //ClassStartTime = new TimeSpan(8, 45, 0),
                //ClassEndTime = new TimeSpan(15, 15, 0),
            };

            return View(viewModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnsureDiscriminatorClaim(Discriminator.Instructor)]
        public ActionResult Create(CreateCourseViewModel model)
        {
            if (model == null)
            {
                ModelState.AddModelError("", "No form data found.");
                (SelectList schoolPrograms, SelectList classRooms) = BuildCreateViewModel();
                model.ClassRooms = classRooms;
                model.SchoolPrograms = schoolPrograms;
                return View(model);
            }
            if (model.ClassStartTime == default)
            {
                ModelState.AddModelError(nameof(model.ClassStartTime), "You must provide a valid Class Start Time");
            }
            if (model.ClassEndTime == default)
            {
                ModelState.AddModelError(nameof(model.ClassStartTime), "You must provide a valid Class End Time");
            }
            if (!ModelState.IsValid)
            {
                (SelectList schoolPrograms, SelectList classRooms) = BuildCreateViewModel();
                model.ClassRooms = classRooms;
                model.SchoolPrograms = schoolPrograms;
                return View(model);
            }

            model.Name = model.Name.Trim();

            string userId = User.Identity.GetUserId();

            if (DbContext.Users.First(user => user.Id == userId) is Instructor foundUser)
            {
                SchoolProgram foundSchoolProgram = DbContext.SchoolPrograms.First(sp => sp.Id == model.SchoolProgramId);
                if (foundSchoolProgram.Courses.Any(course => course.Name.ToLower() == model.Name.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.Name), $"The course name ({model.Name}) is already taken in the school program that you have selected ({foundSchoolProgram.Name})");
                    (SelectList schoolPrograms, SelectList classRooms) = BuildCreateViewModel();
                    model.ClassRooms = classRooms;
                    model.SchoolPrograms = schoolPrograms;
                    return View(model);
                }

                Course newCourse = new Course
                {
                    Name = model.Name,
                    StartDate = model.StartDate.Value,
                    EndDate = model.EndDate.Value,


                    ClassStartTime = model.ClassStartTime,//new TimeSpan(8, 45, 0),
                    ClassEndTime = model.ClassEndTime,//new TimeSpan(13, 15, 0),


                    SchoolProgramId = model.SchoolProgramId.Value,
                    ClassRoomId = model.ClassRoomId.Value,
                    InstructorId = userId,
                };
                DbContext.Courses.Add(newCourse);
                DbContext.SaveChanges();

                return RedirectToAction(nameof(Details), new { newCourse.Id });
            }

            ModelState.AddModelError("", $"You must be a {nameof(Discriminator.Instructor)}");
            return View("Error");
        }

        [HttpGet]
        [EnsureDiscriminatorClaim(Discriminator.Instructor)]
        public ActionResult Edit(Guid id)
        {
            Course foundCourse = DbContext.Courses.FirstOrDefault(course => course.Id == id);

            if (foundCourse == null)
            {
                ModelState.AddModelError("", "Course was not found");
                return View("Error");
            }

            string userId = User.Identity.GetUserId();
            if (foundCourse.InstructorId != userId)
            {
                ModelState.AddModelError("", "You can't edit a course that you don't belong to in any way");
                return View("Error");
            }

            EditCourseViewModel viewModel = new EditCourseViewModel
            {
                Name = foundCourse.Name,
                StartDate = foundCourse.StartDate,
                EndDate = foundCourse.EndDate,
                ClassStartTime = foundCourse.ClassStartTime,
                ClassEndTime = foundCourse.ClassEndTime
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [EnsureDiscriminatorClaim(Discriminator.Instructor)]
        public ActionResult Edit(Guid id, EditCourseViewModel model)
        {
            if (model == null)
            {
                ModelState.AddModelError("", "No form data found.");
                return View(model);
            }
            if (model.ClassStartTime == default)
            {
                ModelState.AddModelError(nameof(model.ClassStartTime), "You must provide a valid Class Start Time");
            }
            if (model.ClassEndTime == default)
            {
                ModelState.AddModelError(nameof(model.ClassStartTime), "You must provide a valid Class End Time");
            }
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            model.Name = model.Name.Trim();
            string userId = User.Identity.GetUserId();

            if (User.Identity.GetUserDiscriminator() == Discriminator.Instructor)
            {
                Course foundCourse = DbContext.Courses.First(sp => sp.Id == id);
                if (foundCourse.InstructorId != userId)
                {
                    ModelState.AddModelError("", "You can't edit a course that you don't belong to");
                    return View("Error");
                }

                if (foundCourse.SchoolProgram.Courses.Any(course => course.Name.ToLower() == model.Name.ToLower()))
                {
                    ModelState.AddModelError(nameof(model.Name), $"The course name ({model.Name}) is already taken in the school program that you have selected ({foundCourse.Name})");
                    return View(model);
                }

                foundCourse.Name = model.Name;
                foundCourse.StartDate = model.StartDate;
                foundCourse.EndDate = model.EndDate;
                foundCourse.ClassStartTime = model.ClassStartTime;
                foundCourse.ClassEndTime = model.ClassEndTime;

                DbContext.SaveChanges();

                return RedirectToAction(nameof(Details), new { id });
            }

            ModelState.AddModelError("", $"You must be a {nameof(Discriminator.Instructor)}");
            return View("Error");
        }

        [HttpGet]
        [EnsureDiscriminatorClaim(Discriminator.Instructor, Discriminator.Student)]
        public ActionResult Details(Guid id)
        {
            Course foundCourse = DbContext.Courses
                .Include(course => course.EnrolledStudents)
                .Include(course => course.Instructor)
                .Include(course => course.SchoolProgram)
                .Include(course => course.ClassRoom)
                .FirstOrDefault(course => course.Id == id);

            if (foundCourse == null)
            {
                ModelState.AddModelError("", "Course was not found");
                return View("Error");
            }

            string userId = User.Identity.GetUserId();
            if (foundCourse.InstructorId != userId && !foundCourse.EnrolledStudents.Any(student => student.Id == userId))
            {
                ModelState.AddModelError("", "You can't view a course that you don't belong to in any way");
                return View("Error");
            }

            ViewBag.Course = foundCourse;

            return View();
        }
    }
}