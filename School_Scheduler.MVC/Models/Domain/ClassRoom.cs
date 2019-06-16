using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace School_Scheduler.Models.Domain
{
    public class ClassRoom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int RoomNumber { get; set; }


        public virtual List<Course> Courses { get; set; }
        public ClassRoom() => Courses = new List<Course>();
    }
    public class ClassRoomConfig : EntityTypeConfiguration<ClassRoom>
    {
        public ClassRoomConfig()
        {
            HasKey(cr => cr.Id);

            Property(cr => cr.Name).IsRequired();
            Property(cr => cr.RoomNumber).IsRequired();

            HasMany(cr => cr.Courses)
                .WithRequired(c => c.Room)
                .HasForeignKey(c => c.RoomId);
        }
    }
}