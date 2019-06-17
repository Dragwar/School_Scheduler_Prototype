using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace School_Scheduler.MVC.Models.Domain
{
    /// <summary>
    /// Represents a person who teaches <see cref="Course"/>s and belongs to <see cref="SchoolProgram"/>s
    /// </summary>
    public class Instructor
    {
        /// <summary>
        /// Represents the <see cref="Guid"/> <see cref="Id"/> of <see cref="Instructor"/> (Don't show to users)
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Represents the <see cref="string"/> Name of a <see cref="Instructor"/>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Represents a unique <see cref="int"/> number (auto-generated)
        /// </summary>
        public int InstructorNumber { get; set; }

        /// <summary>
        /// Navigational property to the <see cref="Instructor"/>'s <see cref="Courses"/> (That he/she will teach)
        /// </summary>
        public virtual List<Course> Courses { get; set; }

        /// <summary>
        /// Navigational property to the <see cref="Instructor"/>'s <see cref="SchoolPrograms"/>
        /// </summary>        
        public virtual List<SchoolProgram> SchoolPrograms { get; set; }

        /// <summary>
        /// Creates a new <see cref="Instructor"/> with a empty courses and school programs List and a unique <see cref="Guid"/> <see cref="Id"/>
        /// </summary>
        public Instructor()
        {
            Id = Guid.NewGuid();
            Courses = new List<Course>();
            SchoolPrograms = new List<SchoolProgram>();
        }

        public override string ToString() => $"Instructor: {Name}, {InstructorNumber}";
    }
    public class InstructorConfig : EntityTypeConfiguration<Instructor>
    {
        public const int MaxNameLength = 250;
        public InstructorConfig()
        {
            HasKey(i => i.Id)
                .Property(i => i.Id)
                .IsRequired();

            Property(i => i.InstructorNumber)
                .IsRequired();

            Property(i => i.Name)
                .HasMaxLength(MaxNameLength)
                .IsRequired();


            HasMany(i => i.Courses)
                .WithRequired(c => c.Instructor)
                .HasForeignKey(c => c.InstructorId);

            HasMany(i => i.SchoolPrograms)
                .WithMany(sp => sp.Instructors);
        }
    }
}
