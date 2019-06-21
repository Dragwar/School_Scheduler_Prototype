using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace School_Scheduler.MVC.Models.ViewModels
{
    public class CreateCourseViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public DateTime? StartDate { get; set; }

        [Required]
        public DateTime? EndDate { get; set; }

        [Required]
        public TimeSpan ClassStartTime { get; set; }// = new TimeSpan(8, 45, 0);

        [Required]
        public TimeSpan ClassEndTime { get; set; }// = new TimeSpan(15, 15, 0);

        public SelectList ClassRooms { get; set; }

        [Required]
        public Guid? ClassRoomId { get; set; }
        public SelectList SchoolPrograms { get; set; }

        [Required]
        public Guid? SchoolProgramId { get; set; }
    }
}