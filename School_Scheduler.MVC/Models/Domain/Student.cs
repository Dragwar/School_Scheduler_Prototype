using System;
using System.Data.Entity.ModelConfiguration;

namespace School_Scheduler.MVC.Models.Domain
{
    /// <summary>
    /// Represents a person who participates in <see cref="Course"/>s and <see cref="SchoolProgram"/>s
    /// </summary>
    public class Student : ApplicationUser
    {
        /// <summary>
        /// Represents a unique <see cref="int"/> number (auto-generated)
        /// </summary>
        public int StudentNumber { get; set; }

        // TODO: To create a list of passed and failed courses in the student
        /// <Todo>
        /// public List<Course> PassedCourses { get; set; }
        /// public List<Course> FailedCourses { get; set; }
        /// </Todo>

        /// <summary>
        /// Navigational property to the <see cref="Student"/>'s Course
        /// </summary>
        public virtual Course CurrentCourse { get; set; }

        /// <summary>
        /// Foreign Key for the <see cref="Course"/> navigational property
        /// </summary>
        public Guid CurrentCourseId { get; set; }

        /// <summary>
        /// Navigational property to the <see cref="Student"/>'s Current <see cref="SchoolProgram"/>
        /// </summary>
        public virtual SchoolProgram Program { get; set; }

        /// <summary>
        /// Foreign Key for the <see cref="SchoolProgram"/> navigational property
        /// </summary>
        public Guid ProgramId { get; set; }

        public override string ToString() => $"Student: {Name}, {StudentNumber}";
    }

    public class StudentConfig : EntityTypeConfiguration<Student>
    {
        public const int MaxNameLength = 250;
        public StudentConfig()
        {
            HasKey(s => s.Id)
                .Property(s => s.Id)
                .IsRequired();

            Property(s => s.Name)
                .HasMaxLength(MaxNameLength)
                .IsRequired();

            Property(s => s.StudentNumber).IsRequired();



            HasRequired(s => s.Program)
                .WithMany(sp => sp.EnrolledStudents)
                .HasForeignKey(s => s.ProgramId);

            //    HasMany(s => s.Courses)
            //        .WithMany(c => c.EnrolledStudents);

            HasRequired(s => s.CurrentCourse)
                .WithMany(c => c.EnrolledStudents)
                .HasForeignKey(s => s.CurrentCourseId)
                .WillCascadeOnDelete(false);
        }
    }
}