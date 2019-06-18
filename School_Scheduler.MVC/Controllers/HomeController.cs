using System.Linq;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using School_Scheduler.MVC.Models;
using School_Scheduler.MVC.Models.Domain;
using School_Scheduler.MVC.Models.ViewModels;

namespace School_Scheduler.MVC.Controllers
{
    public static class Helper
    {
        public static bool IsType<TAppUserType>(this ApplicationUser userToCheck)
            where TAppUserType : ApplicationUser => userToCheck is TAppUserType;
        public static TAppUserType AsType<TAppUserType>(this ApplicationUser userToCheck)
            where TAppUserType : ApplicationUser => userToCheck as TAppUserType;
    }

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationDbContext db = new ApplicationDbContext();


            ApplicationUser foundCurrentUser = db.Users.FirstOrDefault(user => user.Id == currentUserId);

            bool isInstructor = foundCurrentUser.IsType<Instructor>();
            bool isStudent = foundCurrentUser.IsType<Student>();

            if (isInstructor)
            {
                SchoolProgram schoolProgram = ((Instructor)foundCurrentUser).SchoolProgram;
                CalenderDataViewModel viewModel = new CalenderDataViewModel(schoolProgram);
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