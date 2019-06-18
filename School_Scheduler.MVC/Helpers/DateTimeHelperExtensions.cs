using System;
using System.Linq;
using School_Scheduler.MVC.Models.Domain;
using School_Scheduler.MVC.Models.ViewModels;

namespace School_Scheduler.MVC.Helpers
{
    public static class DateTimeHelperExtensions
    {
        public static bool InRange(this DateTime dateToCheck, DateTime startDate, DateTime endDate) => dateToCheck >= startDate && dateToCheck < endDate;
        public static bool InRange(this DateTime dateToCheck, Course course) => dateToCheck >= course.StartDate && dateToCheck < course.EndDate;
        public static bool InRange(this DateTime dateToCheck, CourseViewModel course) => dateToCheck >= course.StartDate && dateToCheck < course.EndDate;
        public static bool InRange(this DateTime dateToCheck, SchoolProgram program)
        {
            DateTime firstDateTime = program.Courses
                .Select(course => course.StartDate)
                .OrderBy(dt => dt)
                .First();

            DateTime lastDateTime = program.Courses
                .Select(course => course.StartDate)
                .OrderBy(dt => dt)
                .Last();

            return dateToCheck.InRange(firstDateTime, lastDateTime);
        }
    }
}