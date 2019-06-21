using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace School_Scheduler.MVC.Models.ViewModels
{
    public class IndexForStudentViewModel
    {
        public DateTime Today { get; set; } = DateTime.Now;
        public CourseViewModel CurrentCourse { get; set; }
        
        public IndexForStudentViewModel()
        {
            Today = DateTime.Now;
        }        
    }
}