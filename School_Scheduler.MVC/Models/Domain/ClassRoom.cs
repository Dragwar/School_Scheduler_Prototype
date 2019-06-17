using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;

namespace School_Scheduler.MVC.Models.Domain
{
    /// <summary>
    /// Represents a class room of a school with Unique a <see cref="RoomNumber"/>
    /// </summary>
    /// <remarks>
    /// <see cref="ClassRoom"/>s can have <see cref="Course"/>s held within them
    /// so use <see cref="Courses"/> navigational property to view a <see cref="ClassRoom"/>'s courses.
    /// In other words you can see the All the <see cref="Course"/>s that will be using <see cref="ClassRoom" />
    /// </remarks>
    public class ClassRoom
    {
        /// <summary>
        /// Represents the <see cref="Guid"/> <see cref="Id"/> of <see cref="ClassRoom"/> (Don't show to users)
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Represents the <see cref="string"/> Name of a <see cref="ClassRoom"/>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///Represents the<see cref= "int" /> RoomNumber of a <see cref = "ClassRoom" />
        /// </summary>
        public int RoomNumber { get; set; }

        /// <summary>
        /// Navigational property to the <see cref="ClassRoom"/>'s Courses
        /// </summary>
        public virtual List<Course> Courses { get; set; }

        /// <summary>
        /// Creates a new <see cref="ClassRoom"/> with a empty Courses List and a unique <see cref="Guid"/> <see cref="Id"/>
        /// </summary>        
        public ClassRoom()
        {
            Id = Guid.NewGuid();
            Courses = new List<Course>();
        }

        public override string ToString() => $"ClassRoom: {Name}, {RoomNumber} ";
    }
    public class ClassRoomConfig : EntityTypeConfiguration<ClassRoom>
    {
        public const int MaxNameLength = 250;
        public ClassRoomConfig()
        {
            HasKey(cr => cr.Id)
                .Property(c => c.Id)
                .IsRequired();

            Property(cr => cr.Name)
                .HasMaxLength(MaxNameLength)
                .IsRequired();

            Property(cr => cr.RoomNumber).IsRequired();

            HasMany(cr => cr.Courses)
                .WithRequired(c => c.Room)
                .HasForeignKey(c => c.RoomId);
        }
    }
}