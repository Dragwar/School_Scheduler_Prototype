using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using School_Scheduler.MVC.Models;
using School_Scheduler.MVC.Models.Domain;
using School_Scheduler.MVC.Models.ViewModels;
using static School_Scheduler.MVC.Helpers.UserIdentityExtensions;

namespace School_Scheduler.MVC.Controllers
{
    public static class Helper
    {
        //public static Discriminator GetUserDiscriminator(this )
        //{

        //}
        public static bool IsType<TAppUserType>(this ApplicationUser userToCheck)
            where TAppUserType : ApplicationUser => userToCheck is TAppUserType;
        public static bool IsType<TAppUserType>(this ApplicationUser userToCheck, out TAppUserType appUser)
            where TAppUserType : ApplicationUser
        {
            if (userToCheck is TAppUserType user)
            {
                appUser = user;
                return true;
            }

            appUser = null;
            return false;
        }
        public static TAppUserType AsType<TAppUserType>(this ApplicationUser userToCheck)
            where TAppUserType : ApplicationUser => userToCheck as TAppUserType;
    }
    class IndexPost { public DateTime DateTime { get; set; } }
    public class HomeController : Controller
    {
        public ActionResult Index(CalenderDataViewModel model)
        {
            string currentUserId = User.Identity.GetUserId();
            // Discriminator discriminator = User.Identity.GetDiscriminator();
            ApplicationDbContext db = new ApplicationDbContext();


            ApplicationUser foundCurrentUser = db.Users.FirstOrDefault(user => user.Id == currentUserId);

            bool isInstructor = foundCurrentUser.IsType<Instructor>();
            bool isStudent = foundCurrentUser.IsType<Student>();

            if (isInstructor)
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