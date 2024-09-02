using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    //public class EmployeeMonthlyReportViewModel
    //{
    //    public string EmployeeName { get; set; }
    //    public string Department { get; set; }
    //    public decimal BasicSalary { get; set; }
    //    public int AttendedDays { get; set; }
    //    public int AbsentDays { get; set; }
    //    public decimal BonusesByHours { get; set; }
    //    public decimal DeductionsByHours { get; set; }
    //    public decimal NetSalary { get; set; }
    //}

    public class DailyAttendanceDetail
    {
        public DateTime Date { get; set; }
        public bool IsHoliday { get; set; }
        public bool IsWeeklyHoliday { get; set; }
        public bool Attended { get; set; }
        public TimeSpan? ArrivalTime { get; set; }
        public TimeSpan? DepartureTime { get; set; }

        public decimal? Deduction { get; set; }
        public decimal? Bonus { get; set; }


    }

    public class EmployeeMonthlyReportViewModel
    {
        public string EmployeeName { get; set; }
        public decimal BasicSalary { get; set; }
        public int AttendedDays { get; set; }
        public int AbsentDays { get; set; }
        public decimal BonusesByHours { get; set; }
        public decimal DeductionsByHours { get; set; }
        public decimal NetSalary { get; set; }

        public List<DailyAttendanceDetail> DailyDetails { get; set; }
    }

}
