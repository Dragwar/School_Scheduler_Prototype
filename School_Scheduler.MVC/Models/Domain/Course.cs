using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace School_Scheduler.Models.Domain
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TimeSpan ClassStartTime { get; set; }
        public TimeSpan ClassEndTime { get; set; }


        public virtual ClassRoom Room { get; set; }
        public int RoomId { get; set; }

        public virtual SchoolProgram SchoolProgram { get; set; }
        public int SchoolProgramId { get; set; }

        public virtual Instructor Instructor { get; set; }
        public int InstructorId { get; set; }
    }
    public class CourseConfig : EntityTypeConfiguration<Course>
    {
        public CourseConfig()
        {
            HasKey(c => c.Id)
                .Property(c => c.Id)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity)
                .IsRequired();

            Property(c => c.ClassStartTime).IsRequired();
            Property(c => c.ClassEndTime).IsRequired();
            Property(c => c.StartDate).IsRequired();
            Property(c => c.EndDate).IsRequired();
            Property(c => c.Name).IsRequired();


            HasRequired(c => c.Instructor)
                .WithMany(i => i.Courses)
                .HasForeignKey(c => c.InstructorId);

            HasRequired(c => c.Room)
                .WithMany(r => r.Courses)
                .HasForeignKey(c => c.RoomId);

            HasRequired(c => c.SchoolProgram)
                .WithMany(sp => sp.Courses)
                .HasForeignKey(c => c.SchoolProgramId);
        }
    }
}