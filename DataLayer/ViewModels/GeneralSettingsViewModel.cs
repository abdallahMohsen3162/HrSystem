using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;
namespace DataLayer.ViewModels
{


    public class GeneralSettingsIndexViewModel
    {
        public List<EmployeeGeneralSettingsViewModel> ?EmployeesWithSettings { get; set; }
        public List<Employee> ?EmployeesWithoutSettings { get; set; }
        public GeneralSettingsViewModel NewGeneralSettings { get; set; }
    }

    public class GeneralSettingsViewModel
    {
        [Required]
        public int EmployeeId { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid number.")]
        public decimal BonusPerHour { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Please enter a valid number.")]
        public decimal RivalPerHour { get; set; }
        public List<string> WeeklyHolidayList { get; set; } = new List<string>();

        public List<string> SelectedDays { get; set; } = new List<string>();

    }



}