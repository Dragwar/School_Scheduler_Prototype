using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace School_Scheduler.MVC.Controllers
{
    public class ClassRoomController : Controller
    {
        // Calender ClassRoomView
        public ActionResult Index()
        {
            return View();
        }
    }
}