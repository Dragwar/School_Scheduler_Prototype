using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace School_Scheduler.MVC.Models.Domain
{
    /// <summary>
    /// Represents a program of a school with Unique a <see cref="SchoolProgram"/>
    /// </summary>
    public class SchoolProgram
    {
        /// <summary>
        /// Represents the <see cref="Guid"/> <see cref="Id"/> of <see cref="SchoolProgram"/> (Don't show to users)
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Represents the <see cref="string"/> Name of a <see cref="SchoolProgram"/>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Navigational property to the <see cref="Instructor"/>'s Courses
        /// </summary>
        public virtual List<Instructor> Instructors { get; set; }

        /// <summary>
        /// Navigational property to the <see cref="Course"/>'s Courses
        /// </summary>
        public virtual List<Course> Courses { get; set; }

        /// <summary>
        /// Navigational property to the <see cref="SchoolProgram"/>'s current enrolled <see cref="Student"/>s
        /// </summary>
        public virtual List<Student> EnrolledStudents { get; set; }

        /// <summary>
        /// Creates a new <see cref="SchoolProgram"/> with a empty instructors and courses List and a unique <see cref="Guid"/>
        /// </summary>
        public SchoolProgram()
        {
            Id = Guid.NewGuid();
            Instructors = new List<Instructor>();
            Courses = new List<Course>();
            EnrolledStudents = new List<Student>();
        }

        public override string ToString() => $"SchoolProgram: {Name}";
    }
    public class SchoolProgramConfig : EntityTypeConfiguration<SchoolProgram>
    {
        public SchoolProgramConfig()
        {
            HasKey(sp => sp.Id)
                .Property(sp => sp.Id)
                .IsRequired();


            HasMany(sp => sp.Courses)
                .WithRequired(c => c.SchoolProgram)
                .HasForeignKey(c => c.SchoolProgramId);

            HasMany(sp => sp.Instructors)
                .WithRequired(i => i.SchoolProgram)
                .HasForeignKey(i => i.SchoolProgramId);

            HasMany(sp => sp.EnrolledStudents)
                .WithRequired(s => s.Program)
                .HasForeignKey(s => s.ProgramId);
        }
    }
}