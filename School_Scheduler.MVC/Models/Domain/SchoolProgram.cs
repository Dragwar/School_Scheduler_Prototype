using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace School_Scheduler.Models.Domain
{
    public class SchoolProgram
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public virtual List<Instructor> Instructors { get; set; }
        public virtual List<Course> Courses { get; set; }
        public SchoolProgram()
        {
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
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();


            HasMany(sp => sp.Courses)
                .WithRequired(c => c.Program)
                .HasForeignKey(c => c.ProgramId);

            HasMany(sp => sp.Instructors)
                .WithMany(i => i.Programs)
                .Map(x => x.ToTable(nameof(Instructor) + "X" + nameof(SchoolProgram)));
        }
    }
}