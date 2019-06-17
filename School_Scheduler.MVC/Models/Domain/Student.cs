using System;
using System.Data.Entity.ModelConfiguration;

namespace School_Scheduler.MVC.Models.Domain
{
    /// <summary>
    /// Represents a person who participates in <see cref="Course"/>s and <see cref="SchoolProgram"/>s
    /// </summary>
    public class Student
    {
        /// <summary>
        /// Represents the <see cref="Guid"/> <see cref="Id"/> of <see cref="Student"/> (Don't show to users)
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Represents the <see cref="string"/> First Name of a <see cref="Student"/>
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Represents the <see cref="string"/> Last Name of a <see cref="Student"/>
        /// </summary>
        public string LastName { get; set; }

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

        /// <summary>
        /// Creates a new <see cref="Student"/> with a empty courses and a unique <see cref="Guid"/> <see cref="Id"/>
        /// </summary>
        public Student() => Id = Guid.NewGuid();

        public override string ToString() => $"Student: {FirstName} {LastName}, {StudentNumber}";
    }

    public class StudentConfig : EntityTypeConfiguration<Student>
    {
        public const int MaxFirstNameLength = 250;
        public const int MaxLastNameLength = 250;
        public StudentConfig()
        {
            HasKey(s => s.Id)
                .Property(s => s.Id)
                .IsRequired();

            Property(s => s.FirstName)
                .HasMaxLength(MaxFirstNameLength)
                .IsRequired();

            Property(s => s.LastName)
                .HasMaxLength(MaxLastNameLength)
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