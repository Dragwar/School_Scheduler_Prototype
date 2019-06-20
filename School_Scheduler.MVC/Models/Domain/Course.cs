using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace School_Scheduler.MVC.Models.Domain
{
    /// <summary>
    /// Represents a school course that is just a part/section of a <see cref="Domain.SchoolProgram"/>
    /// </summary>
    /// <remarks>
    /// To create a course, the user needs to enter the following information:
    /// - Instructor’s name,
    /// - program name,
    /// - course name,
    /// - room number,
    /// - start date
    /// - end date
    /// - class start time
    /// - class end time
    /// </remarks>
    public class Course
    {
        /// <summary>
        /// Represents the <see cref="Guid"/> <see cref="Id"/> of <see cref="Course"/> (Don't show to users)
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Represents the <see cref="string"/> Name of a <see cref="Course"/>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Represents a unique <see cref="int"/> number (auto-generated)
        /// </summary>
        public int CourseNumber { get; set; }

        /// <summary>
        /// Represents the <see cref="DateTime"/> StartDate of a <see cref="Course"/>
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Represents the <see cref="DateTime"/> EndDate of a <see cref="Course"/>
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Represents the <see cref="TimeSpan"/> in which the class will Start
        /// </summary>
        public TimeSpan ClassStartTime { get; set; }

        /// <summary>
        /// Represents the <see cref="TimeSpan"/> in which the class will End
        /// </summary>
        public TimeSpan ClassEndTime { get; set; }

        /// <summary>
        /// Navigational property to the <see cref="Domain.ClassRoom"/> that this <see cref="Course"/> takes place at/in
        /// </summary>
        public virtual ClassRoom ClassRoom { get; set; }

        /// <summary>
        /// Foreign Key for the <see cref="ClassRoom"/> navigational property
        /// </summary>
        public Guid ClassRoomId { get; set; }

        /// <summary>
        /// Navigational property to the <see cref="Domain.SchoolProgram"/> that this <see cref="Course"/> belongs to
        /// </summary>
        public virtual SchoolProgram SchoolProgram { get; set; }

        /// <summary>
        /// Foreign Key for the <see cref="SchoolProgram"/> navigational property
        /// </summary> 
        public Guid SchoolProgramId { get; set; }

        /// <summary>
        /// Navigational property to the <see cref="Domain.Instructor"/> who teaches this <see cref="Course"/>
        /// </summary>
        public virtual Instructor Instructor { get; set; }

        /// <summary>
        /// Foreign Key for the <see cref="Instructor"/> navigational property
        /// </summary>       
        public string InstructorId { get; set; }

        /// <summary>
        /// Navigational property to the <see cref="Student"/>s who are enrolled in this <see cref="Course"/>
        /// </summary>
        public virtual List<Student> EnrolledStudents { get; set; }


        // TODO: To create a list of passed and failed students in the course
        /// <Todo>
        /// public List<Student> PassedStudents { get; set; }
        /// public List<Course> FailedStudents { get; set; }
        /// </Todo>


        /// <summary>
        /// Creates a new <see cref="Course"/> with a unique <see cref="Guid"/> <see cref="Id"/>
        /// </summary>
        public Course()
        {
            Id = Guid.NewGuid();
            EnrolledStudents = new List<Student>();
        }

        public override string ToString() => $"Course: {Name}, {CourseNumber}";
    }
    public class CourseConfig : EntityTypeConfiguration<Course>
    {
        public const int MaxNameLength = 250;
        public CourseConfig()
        {
            Property(c => c.CourseNumber)
                .IsRequired();

            Property(c => c.ClassStartTime).IsRequired();
            Property(c => c.ClassEndTime).IsRequired();
            Property(c => c.StartDate).IsRequired();
            Property(c => c.EndDate).IsRequired();

            Property(c => c.Name)
                .HasMaxLength(MaxNameLength)
                .IsRequired();


            HasRequired(c => c.Instructor)
                .WithMany(i => i.Courses)
                .HasForeignKey(c => c.InstructorId);

            HasRequired(c => c.ClassRoom)
                .WithMany(r => r.Courses)
                .HasForeignKey(c => c.ClassRoomId);

            HasRequired(c => c.SchoolProgram)
                .WithMany(sp => sp.Courses)
                .HasForeignKey(c => c.SchoolProgramId);

            //HasMany(c => c.EnrolledStudents)
            //    .WithMany(s => s.Courses);

            HasMany(c => c.EnrolledStudents)
                .WithOptional(s => s.CurrentCourse)
                .HasForeignKey(s => s.CurrentCourseId);
        }
    }
}