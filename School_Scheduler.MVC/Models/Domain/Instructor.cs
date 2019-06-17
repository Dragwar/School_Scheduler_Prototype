using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace School_Scheduler.Models.Domain
{
    public class Instructor
    {
        public int Id { get; set; }
        public string Name { get; set; }


        public virtual List<Course> Courses { get; set; }
        public virtual List<SchoolProgram> SchoolPrograms { get; set; }
        public Instructor()
        {
            Courses = new List<Course>();
            SchoolPrograms = new List<SchoolProgram>();
        }
    }
    public class InstructorConfig : EntityTypeConfiguration<Instructor>
    {
        public InstructorConfig()
        {
            HasKey(i => i.Id)
                .Property(i => i.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(i => i.Name).IsRequired();


            HasMany(i => i.Courses)
                .WithRequired(c => c.Instructor)
                .HasForeignKey(c => c.InstructorId);

            HasMany(i => i.SchoolPrograms)
                .WithMany(sp => sp.Instructors)
                .Map(x => x.ToTable(nameof(Instructor) + "X" + nameof(SchoolProgram)));
        }
    }
}
