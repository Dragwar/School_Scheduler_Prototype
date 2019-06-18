using System;
using System.Collections.Generic;
using System.Linq;
using School_Scheduler.MVC.Models.Domain;

namespace School_Scheduler.MVC.Models.ViewModels
{
    public class SchoolProgramViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<InstructorViewModel> Instructors { get; set; }
        public IEnumerable<CourseViewModel> Courses { get; set; }
        public IEnumerable<StudentViewModel> EnrolledStudents { get; set; }

        public SchoolProgramViewModel(SchoolProgram schoolProgram)
        {
            if (schoolProgram == null)
            {
                throw new ArgumentNullException(nameof(schoolProgram));
            }

            Id = schoolProgram.Id;
            Name = schoolProgram.Name;
            Instructors = schoolProgram.Instructors.Select(i => new InstructorViewModel(i));
            Courses = schoolProgram.Courses.Select(c => new CourseViewModel(c));
            EnrolledStudents = schoolProgram.EnrolledStudents.Select(s => new StudentViewModel(s));
        }

        public SchoolProgramViewModel()
        {
        }
        public override string ToString() => $"SchoolProgramViewModel: {Name}";
    }
}