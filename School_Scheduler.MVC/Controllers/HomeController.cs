using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
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
                SchoolProgram schoolProgram = ((Instructor)foundCurrentUser).SchoolProgram;
                CalenderDataViewModel viewModel = new CalenderDataViewModel(schoolProgram);
                if (model != null)
                {
                    viewModel.Today = model.Today;
                }
                return View(viewModel);
            }

            return View("Error");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}