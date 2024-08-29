using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class EmployeeGeneralSettingsViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public string Image { get; set; }

        [Display(Name = "Bonus Per Hour")]
        public decimal BonusPerHour { get; set; }

        [Display(Name = "Rival Per Hour")]
        public decimal RivalPerHour { get; set; }

        [Display(Name = "Weekly Holidays")]
        public string WeeklyHolidays { get; set; }
    }
}
