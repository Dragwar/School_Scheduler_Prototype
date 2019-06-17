using System;
using System.Collections.Generic;
using System.Linq;
using School_Scheduler.MVC.Models.Domain;

namespace School_Scheduler.MVC.Models.ViewModels
{
    public class CalenderDataViewModel
    {
        public DateTime Today { get; set; } = DateTime.Now;
        public List<SchoolProgramViewModel> SchoolPrograms { get; set; }
        public CalenderDataViewModel(List<SchoolProgram> schoolPrograms)
        {
            SchoolPrograms = schoolPrograms
                .Select(sp => new SchoolProgramViewModel(sp))
                .ToList() ?? throw new ArgumentNullException(nameof(schoolPrograms));
        }
    }
}