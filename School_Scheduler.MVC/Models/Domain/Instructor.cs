using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace School_Scheduler.MVC.Models.Domain
{
    /// <summary>
    /// Represents a person who teaches <see cref="Course"/>s and belongs to <see cref="SchoolProgram"/>s
    /// </summary>
    public class Instructor : ApplicationUser
    {
        /// <summary>
        /// Represents a unique <see cref="int"/> number (auto-generated)
        /// </summary>
        public int InstructorNumber { get; set; }

        /// <summary>
        /// Navigational property to the <see cref="Instructor"/>'s <see cref="Courses"/> (That he/she will teach)
        /// </summary>
        public virtual List<Course> Courses { get; set; }

        /// <summary>
        /// Navigational property to the <see cref="Instructor"/>'s <see cref="SchoolProgram"/>
        /// </summary>        
        public virtual SchoolProgram SchoolProgram { get; set; }

        /// <summary>
        /// Foreign Key for the <see cref="Domain.SchoolProgram"/> navigational property
        /// </summary> 
        public Guid? SchoolProgramId { get; set; }

        /// <summary>
        /// Creates a new <see cref="Instructor"/> with a empty courses and a unique <see cref="Guid"/> <see cref="Id"/>
        /// </summary>
        public Instructor()
        {            
            Courses = new List<Course>();
        }

        public override string ToString() => $"Instructor: {Name}, {InstructorNumber}";
    }
    public class InstructorConfig : EntityTypeConfiguration<Instructor>
    {
        public const int MaxNameLength = 250;
        public InstructorConfig()
        {
            Property(i => i.InstructorNumber)
                .IsRequired();

            Property(i => i.Name)
                .HasMaxLength(MaxNameLength)
                .IsRequired();


            HasMany(i => i.Courses)
                .WithRequired(c => c.Instructor)
                .HasForeignKey(c => c.InstructorId);

            HasOptional(i => i.SchoolProgram)
                .WithMany(sp => sp.Instructors)
                .HasForeignKey(i => i.SchoolProgramId);
        }
    }
}
