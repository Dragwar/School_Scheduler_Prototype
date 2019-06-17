using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace School_Scheduler.Models.Domain
{
    public class ClassRoom
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int RoomNumber { get; set; }


        public virtual List<Course> Courses { get; set; }
        public ClassRoom()
        {
            Id = Guid.NewGuid();
            Courses = new List<Course>();
        }
    }
    public class ClassRoomConfig : EntityTypeConfiguration<ClassRoom>
    {
        public ClassRoomConfig()
        {
            HasKey(cr => cr.Id)
                .Property(c => c.Id)
                .IsRequired();

            Property(cr => cr.Name).IsRequired();
            Property(cr => cr.RoomNumber).IsRequired();

            HasMany(cr => cr.Courses)
                .WithRequired(c => c.Room)
                .HasForeignKey(c => c.RoomId);
        }
    }
}