using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace School_Scheduler.MVC.Controllers
{
    public class SchoolProgramController : Controller
    {
        // GET: SchoolProgram
        public ActionResult Index()
        {
            return View();
        }
    }
}