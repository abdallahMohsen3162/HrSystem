using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class EmployeeDetailsViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Image { get; set; }
        public decimal BonusPerHour { get; set; }
        public decimal RivalPerHour { get; set; }
        public string WeeklyHolidays { get; set; }

        public bool SettingsExists { get; set; }
    }
}
