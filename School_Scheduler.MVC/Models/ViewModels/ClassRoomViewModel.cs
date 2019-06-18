using System;
using System.Collections.Generic;
using System.Linq;
using School_Scheduler.MVC.Models.Domain;

namespace School_Scheduler.MVC.Models.ViewModels
{
    public class ClassRoomViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int RoomNumber { get; set; }
        public IEnumerable<CourseViewModel> Courses { get; set; }


        public ClassRoomViewModel(ClassRoom classRoom)
        {
            if (classRoom == null)
            {
                throw new ArgumentNullException(nameof(classRoom));
            }

            Id = classRoom.Id;
            Name = classRoom.Name;
            RoomNumber = classRoom.RoomNumber;
            Courses = classRoom.Courses.Select(cr => new CourseViewModel(cr));
        }

        public ClassRoomViewModel()
        {
        }

        public override string ToString() => $"ClassRoomViewModel: {Name}, {RoomNumber} ";
    }
}