using System;
using System.Collections.Generic;
using System.Linq;
using School_Scheduler.MVC.Models.Domain;

namespace School_Scheduler.MVC.Models.ViewModels
{
    public class InstructorViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int InstructorNumber { get; set; }
        public List<CourseViewModel> Courses { get; set; }
        public List<SchoolProgramViewModel> SchoolPrograms { get; set; }

        public InstructorViewModel(Instructor instructor)
        {
            if (instructor == null)
            {
                throw new ArgumentNullException(nameof(instructor));
            }

            Id = instructor.Id;
            Name = instructor.Name;
            InstructorNumber = instructor.InstructorNumber;
            Courses = instructor.Courses.Select(c => new CourseViewModel(c)).ToList();
            SchoolPrograms = instructor.SchoolPrograms.Select(sp => new SchoolProgramViewModel(sp)).ToList();
        }
        public InstructorViewModel()
        {
        }

        public override string ToString() => $"InstructorViewModel: {Name}, {InstructorNumber}";
    }
}