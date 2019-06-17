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
        public List<InstructorViewModel> Instructors { get; set; }
        public List<CourseViewModel> Courses { get; set; }
        public List<StudentViewModel> EnrolledStudents { get; set; }

        public SchoolProgramViewModel(SchoolProgram schoolProgram)
        {
            if (schoolProgram == null)
            {
                throw new ArgumentNullException(nameof(schoolProgram));
            }

            Id = schoolProgram.Id;
            Name = schoolProgram.Name;
            Instructors = schoolProgram.Instructors.Select(i => new InstructorViewModel(i)).ToList();
            Courses = schoolProgram.Courses.Select(c => new CourseViewModel(c)).ToList();
            EnrolledStudents = schoolProgram.EnrolledStudents.Select(s => new StudentViewModel(s)).ToList();
        }

        public SchoolProgramViewModel()
        {
        }
        public override string ToString() => $"SchoolProgramViewModel: {Name}";
    }
}