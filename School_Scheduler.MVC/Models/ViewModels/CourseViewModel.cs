using System;
using System.Collections.Generic;
using System.Linq;
using School_Scheduler.MVC.Models.Domain;

namespace School_Scheduler.MVC.Models.ViewModels
{
    public class CourseViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int CourseNumber { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan ClassStartTime { get; set; }
        public TimeSpan ClassEndTime { get; set; }
        public ClassRoomViewModel ClassRoom { get; set; }
        public Guid ClassRoomId { get; set; }
        public SchoolProgramViewModel SchoolProgram { get; set; }
        public Guid SchoolProgramId { get; set; }
        public InstructorViewModel Instructor { get; set; }
        public string InstructorId { get; set; }
        public List<StudentViewModel> EnrolledStudents { get; set; }


        public CourseViewModel(Course course)
        {
            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }

            Id = course.Id;
            Name = course.Name;
            CourseNumber = course.CourseNumber;
            StartDate = course.StartDate;
            EndDate = course.EndDate;
            ClassStartTime = course.ClassStartTime;
            ClassEndTime = course.ClassEndTime;
            ClassRoom = new ClassRoomViewModel(course.ClassRoom);
            ClassRoomId = course.ClassRoomId;
            SchoolProgram = new SchoolProgramViewModel(course.SchoolProgram);
            SchoolProgramId = course.SchoolProgramId;
            Instructor = new InstructorViewModel(course.Instructor);
            InstructorId = course.InstructorId;
            EnrolledStudents = course.EnrolledStudents.Select(s => new StudentViewModel(s)).ToList();
        }
        public CourseViewModel()
        {
        }

        public override string ToString() => $"CourseViewModel: {Name}, {CourseNumber}";
    }
}