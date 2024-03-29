﻿using System;
using School_Scheduler.MVC.Models.Domain;

namespace School_Scheduler.MVC.Models.ViewModels
{
    public class StudentViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int StudentNumber { get; set; }
        public CourseViewModel CurrentCourse { get; set; }
        public Guid? CurrentCourseId { get; set; }
        public SchoolProgramViewModel SchoolProgram { get; set; }
        public Guid? SchoolProgramId { get; set; }

        public StudentViewModel(Student student)
        {
            if (student == null)
            {
                throw new ArgumentNullException(nameof(student));
            }

            Id = student.Id;
            Name = student.Name;
            StudentNumber = student.StudentNumber;
            CurrentCourse = student.CurrentCourse != null ? new CourseViewModel(student.CurrentCourse) : null;
            CurrentCourseId = student.CurrentCourseId;
            SchoolProgram = student.SchoolProgram != null ? new SchoolProgramViewModel(student.SchoolProgram) : null;
            SchoolProgramId = student.SchoolProgramId;
        }
        public StudentViewModel()
        {
        }

        public override string ToString() => $"StudentViewModel: {Name}, {StudentNumber}";
    }
}