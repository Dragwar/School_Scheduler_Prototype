using System;
using School_Scheduler.MVC.Models.Domain;

namespace School_Scheduler.MVC.Models.ViewModels
{
    public class CalenderDataViewModel
    {
        public DateTime Today { get; set; } = DateTime.Now;
        public SchoolProgramViewModel SchoolProgram { get; set; }
        public CalenderDataViewModel(SchoolProgram schoolPrograms)
        {
            if (schoolPrograms == null)
            {
                throw new ArgumentNullException(nameof(schoolPrograms));
            }

            SchoolProgram = new SchoolProgramViewModel(schoolPrograms);
        }
    }
}