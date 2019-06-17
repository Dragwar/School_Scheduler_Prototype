using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using School_Scheduler.MVC.Models;
using School_Scheduler.MVC.Models.Domain;

namespace School_Scheduler.MVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (ApplicationDbContext db = new ApplicationDbContext())
            {
                List<SchoolProgram> list = db.SchoolPrograms
                    .Include(sp => sp.Courses)
                    .Include(sp => sp.EnrolledStudents)
                    .Include(sp => sp.Instructors)
                    .ToList();

                ViewBag.List = list;
            }
            return View();
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