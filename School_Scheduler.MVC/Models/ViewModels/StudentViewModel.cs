using System;
using School_Scheduler.MVC.Models.Domain;

namespace School_Scheduler.MVC.Models.ViewModels
{
    public class StudentViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
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
            Name = student.Name;
            StudentNumber = student.StudentNumber;
            CurrentCourse = new CourseViewModel(student.CurrentCourse);
            CurrentCourseId = student.CurrentCourseId;
            Program = new SchoolProgramViewModel(student.Program);
            ProgramId = student.ProgramId;
        }
        public StudentViewModel()
        {
        }

        public override string ToString() => $"StudentViewModel: {Name}, {StudentNumber}";
    }
}