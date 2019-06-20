using System.Collections.Generic;
using System.Linq;
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


        public ActionResult FullCalendar()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            SchoolProgram program = db.SchoolPrograms.FirstOrDefault();
            ViewBag.SP = program;
            return View();
        }

        public JsonResult GetEvents()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                SchoolProgram program = db.SchoolPrograms.FirstOrDefault();
                List<FullCalendarEventViewModel> events = program.Courses.Select((c, i) => new FullCalendarEventViewModel
                {
                    EventId = i,
                    Start = c.StartDate,
                    End = c.EndDate,
                    Description = $"{c.ClassRoom.Name} class room (Description)",
                    IsFullDay = c.EndDate == default || c.EndDate == c.StartDate,
                    Subject = $"My Subject ({i})",
                    ThemeColor = "green"
                }).ToList();
                return new JsonResult { Data = events, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
    }
}