using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace School_Scheduler.Models.Domain
{
    public class SchoolProgram
    {
        public Guid Id { get; set; }
        public string Name { get; set; }


        public virtual List<Instructor> Instructors { get; set; }
        public virtual List<Course> Courses { get; set; }
        public SchoolProgram()
        {
            Id = Guid.NewGuid();
            Instructors = new List<Instructor>();
            Courses = new List<Course>();
        }
    }
    public class SchoolProgramsConfig : EntityTypeConfiguration<SchoolProgram>
    {
        public SchoolProgramsConfig()
        {
            HasKey(sp => sp.Id)
                .Property(sp => sp.Id)
                .IsRequired();


            HasMany(sp => sp.Courses)
                .WithRequired(c => c.SchoolProgram)
                .HasForeignKey(c => c.SchoolProgramId);

            HasMany(sp => sp.Instructors)
                .WithMany(i => i.SchoolPrograms)
                .Map(x => x.ToTable(nameof(Instructor) + "X" + nameof(SchoolProgram)));
        }
    }
}