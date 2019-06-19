using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace School_Scheduler.MVC.Controllers
{
    public class StudentController : Controller
    {
        // Calender StudentView
        public ActionResult Index()
        {
            return View();
        }
    }
}