using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School_Scheduler.MVC.Models.ViewModels
{
    public class CoursesByDateViewModel
    {
        public List<CourseViewModel> Courses { get; set; }
        public DateTime Today { get; set; }

        public CoursesByDateViewModel()
        {
            Today = DateTime.Now;
        }
    }
}