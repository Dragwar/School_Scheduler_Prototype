using System;
using System.Linq;
using School_Scheduler.MVC.Models.Domain;
using School_Scheduler.MVC.Models.ViewModels;

namespace School_Scheduler.MVC.Helpers
{
    /// <summary>
    /// <see cref="DateTime"/> extensions
    /// </summary>
    public static class DateTimeHelperExtensions
    {
        /// <summary>
        /// Returns <see langword="true"/> if the <paramref name="dateToCheck"/> is between <paramref name="startDate"/> and the <paramref name="endDate"/>
        /// </summary>
        /// <param name="dateToCheck">The DateTime to that has to be between <paramref name="startDate"/> and <paramref name="endDate"/></param>
        /// <param name="startDate"><paramref name="dateToCheck"/> has to be BEFORE this DateTime (<paramref name="startDate"/>) to return <see langword="true"/></param>
        /// <param name="endDate"><paramref name="dateToCheck"/> has to be AFTER this DateTime (<paramref name="endDate"/>) to return <see langword="true"/></param>
        /// <returns><see langword="true"/> if the <paramref name="dateToCheck"/> is after the <paramref name="startDate"/> and before <paramref name="endDate"/></returns>
        public static bool InRange(this DateTime dateToCheck, DateTime startDate, DateTime endDate) => dateToCheck >= startDate && dateToCheck < endDate;

        /// <summary>
        /// Returns <see langword="true"/> if the <paramref name="dateToCheck"/> is between <paramref name="course"/>'s <see cref="Course.StartDate"/> and the <paramref name="course"/>'s <see cref="Course.EndDate"/>
        /// </summary>
        /// <param name="dateToCheck">The DateTime to that has to be between <paramref name="course"/>'s <see cref="Course.StartDate"/> and <paramref name="course"/>'s <see cref="Course.EndDate"/></param>
        /// <param name="course"><see cref="Course"/>'s <see cref="Course.StartDate"/> and <see cref="Course.EndDate"/> will be used</param>
        /// <returns><see langword="true"/> if the <paramref name="dateToCheck"/> is after the <paramref name="course"/>'s <see cref="Course.StartDate"/> and before <paramref name="course"/>'s <see cref="Course.EndDate"/></returns>
        public static bool InRange(this DateTime dateToCheck, Course course) => dateToCheck >= course.StartDate && dateToCheck < course.EndDate;

        /// <summary>
        /// Returns <see langword="true"/> if the <paramref name="dateToCheck"/> is between <paramref name="course"/>'s <see cref="Course.StartDate"/> and the <paramref name="course"/>'s <see cref="Course.EndDate"/>
        /// </summary>
        /// <param name="dateToCheck">The DateTime to that has to be between <paramref name="course"/>'s <see cref="CourseViewModel.StartDate"/> and <paramref name="course"/>'s <see cref="CourseViewModel.EndDate"/></param>
        /// <param name="course"><see cref="Course"/>'s <see cref="CourseViewModel.StartDate"/> and <see cref="CourseViewModel.EndDate"/> will be used</param>
        /// <returns><see langword="true"/> if the <paramref name="dateToCheck"/> is after the <paramref name="course"/>'s <see cref="Course.StartDate"/> and before <paramref name="course"/>'s <see cref="CourseViewModel.EndDate"/></returns>
        public static bool InRange(this DateTime dateToCheck, CourseViewModel course) => dateToCheck >= course.StartDate && dateToCheck < course.EndDate;

        /// <summary>
        /// Returns <see langword="true"/> if the <paramref name="dateToCheck"/> is between <paramref name="course"/>'s <see cref="Course.StartDate"/> and the <paramref name="course"/>'s <see cref="Course.EndDate"/>
        /// </summary>
        /// <param name="dateToCheck">The DateTime to that has to be between <paramref name="course"/>'s <see cref="CourseViewModel.StartDate"/> and <paramref name="course"/>'s <see cref="CourseViewModel.EndDate"/></param>
        /// <param name="course"><paramref name="program"/>'s <see cref="SchoolProgram.Courses"/> oldest and newest <see cref="Course.StartDate"/></param>
        /// <returns><see langword="true"/> if the <paramref name="dateToCheck"/> is after the <paramref name="course"/>'s <see cref="Course.StartDate"/> and before <paramref name="course"/>'s <see cref="CourseViewModel.EndDate"/></returns>
        //public static bool InRange(this DateTime dateToCheck, SchoolProgram program)
        //{
        //    DateTime firstDateTime = program.Courses
        //        .Select(course => course.StartDate)
        //        .OrderBy(dt => dt)
        //        .First();

        //    DateTime lastDateTime = program.Courses
        //        .Select(course => course.StartDate)
        //        .OrderBy(dt => dt)
        //        .Last();

        //    return dateToCheck.InRange(firstDateTime, lastDateTime);
        //}
    }
}