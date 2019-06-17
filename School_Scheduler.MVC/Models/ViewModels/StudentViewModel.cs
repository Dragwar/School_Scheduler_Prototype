using System;
using School_Scheduler.MVC.Models.Domain;

namespace School_Scheduler.MVC.Models.ViewModels
{
    public class StudentViewModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int StudentNumber { get; set; }
        public CourseViewModel CurrentCourse { get; set; }
        public Guid CurrentCourseId { get; set; }
        public SchoolProgramViewModel Program { get; set; }
        public Guid ProgramId { get; set; }

        public StudentViewModel(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            Id = student.Id;
            FirstName = student.FirstName;
            LastName = student.LastName;
            StudentNumber = student.StudentNumber;
            CurrentCourse = new CourseViewModel(student.CurrentCourse);
            CurrentCourseId = student.CurrentCourseId;
            Program = new SchoolProgramViewModel(student.Program);
            ProgramId = student.ProgramId;
        }
        public StudentViewModel()
        {
        }

        public override string ToString() => $"StudentViewModel: {FirstName} {LastName}, {StudentNumber}";
    }
}