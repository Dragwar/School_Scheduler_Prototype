using System;
using System.ComponentModel.DataAnnotations;

namespace School_Scheduler.MVC.Models.ViewModels
{
    public class EditCourseViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public TimeSpan ClassStartTime { get; set; }

        [Required]
        public TimeSpan ClassEndTime { get; set; }
    }
}