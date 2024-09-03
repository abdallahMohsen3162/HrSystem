using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.ViewModels;
using DataLayer.Entities;
using DataLayer.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace YourNamespace.Controllers
{
    public class ReportController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ReportController(ApplicationDbContext context)
        {
            _context = context;
        }



        [HttpGet]
        public IActionResult Index()
        {
            var employees = _context.Employee.ToList(); 
            ViewData["Employees"] = new SelectList(employees, "Id", "EmployeeName"); 
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> MultiReport(int month = -1, int year = -1)
        {
            if(month == -1 || year == -1)
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
            }
            var employees = await _context.Employee
                .Include(e => e.GeneralSettings)
                .ToListAsync();

            var holidays = await _context.Holidays
                .Where(h => h.Date.Month == month && h.Date.Year == year)
                .Select(h => h.Date)
                .ToListAsync();

            var monthlyReports = new List<EmployeeMonthlyReportViewModel>();

            foreach (var employee in employees)
            {
                decimal bonusesByHours = 0;
                decimal deductionsByHours = 0;
                int attendedDays = 0;
                int absentDays = 0;
                var dailyDetails = new List<DailyAttendanceDetail>();

                var weeklyHolidays = employee.GeneralSettings.WeeklyHolidayList;
                int daysInMonth = DateTime.DaysInMonth(year, month);

                for (int day = 1; day <= daysInMonth; day++)
                {
                    var currentDate = new DateTime(year, month, day);
                    var attendanceRecord = _context.AttendanceTables
                        .FirstOrDefault(a => a.EmployeeId == employee.Id && a.Date.Date == currentDate.Date);

                    bool isWeeklyHoliday = weeklyHolidays.Contains((Day)currentDate.DayOfWeek);
                    bool isHoliday = holidays.Contains(currentDate);

                    var detail = new DailyAttendanceDetail
                    {
                        Date = currentDate,
                        IsHoliday = isHoliday,
                        IsWeeklyHoliday = isWeeklyHoliday,
                        Attended = attendanceRecord != null
                    };

                    if (attendanceRecord != null)
                    {
                        attendedDays++;
                        var workedHours = (attendanceRecord.DepartureTime - attendanceRecord.AttendanceTime)?.TotalHours ?? 0;
                        detail.ArrivalTime = attendanceRecord.AttendanceTime;
                        detail.DepartureTime = attendanceRecord.DepartureTime;

                        if (attendanceRecord.AttendanceTime > employee.AttendanceTime)
                        {
                            var lateHours = (attendanceRecord.AttendanceTime - employee.AttendanceTime).TotalHours;
                            deductionsByHours += (decimal)lateHours * employee.GeneralSettings.rivalPerHour;
                            detail.Deduction = (decimal)lateHours * employee.GeneralSettings.rivalPerHour;
                        }
                        else if (attendanceRecord.AttendanceTime < employee.AttendanceTime)
                        {
                            var earlyHours = (employee.AttendanceTime - attendanceRecord.AttendanceTime).TotalHours;
                            bonusesByHours += (decimal)earlyHours * employee.GeneralSettings.bonusPerHoure;
                            detail.Bonus = (decimal)earlyHours * employee.GeneralSettings.bonusPerHoure;
                        }

                        if (attendanceRecord.DepartureTime < employee.DepartureTime)
                        {
                            var earlyLeaveHours = (employee.DepartureTime - attendanceRecord.DepartureTime)?.TotalHours;
                            deductionsByHours += (decimal)earlyLeaveHours * employee.GeneralSettings.rivalPerHour;
                            detail.Deduction += (decimal)earlyLeaveHours * employee.GeneralSettings.rivalPerHour;
                        }
                        else if (attendanceRecord.DepartureTime > employee.DepartureTime)
                        {
                            var lateLeaveHours = (attendanceRecord.DepartureTime - employee.DepartureTime)?.TotalHours;
                            bonusesByHours += (decimal)lateLeaveHours * employee.GeneralSettings.bonusPerHoure;
                            detail.Bonus += (decimal)lateLeaveHours * employee.GeneralSettings.bonusPerHoure;
                        }
                    }
                    else
                    {
                        detail.Attended = false;
                        if (!(isHoliday || isWeeklyHoliday))
                        {
                            absentDays++;

                            var missedDayHours = (employee.DepartureTime - employee.AttendanceTime).TotalHours;
                            var missedDayDeduction = (decimal)missedDayHours * employee.GeneralSettings.rivalPerHour;
                            deductionsByHours += missedDayDeduction;
                            detail.Deduction = missedDayDeduction;
                        }
                    }

                    dailyDetails.Add(detail);
                }

                decimal HourPrice = (employee.Salary / 30) / (employee.DepartureTime - employee.AttendanceTime).Hours;
                var netSalary = employee.Salary + HourPrice * (bonusesByHours - deductionsByHours);

                if (absentDays == decimal.Zero && attendedDays == decimal.Zero)
                {
                    HourPrice = 0;
                    netSalary = employee.Salary;
                }

                var report = new EmployeeMonthlyReportViewModel
                {
                    EmployeeName = employee.EmployeeName,
                    BasicSalary = employee.Salary,
                    AttendedDays = attendedDays,
                    AbsentDays = absentDays,
                    BonusesByHours = bonusesByHours,
                    DeductionsByHours = deductionsByHours,
                    NetSalary = netSalary,
                    DailyDetails = dailyDetails,
                    HourPrice = HourPrice,
                    BonusesByMoney = bonusesByHours * HourPrice,
                    DeductionsByMoney = deductionsByHours * HourPrice
                };

                monthlyReports.Add(report);
            }

            return View(monthlyReports);
        }


        [HttpGet]
        public async Task<IActionResult> EmployeeMonthlyReport(int month, int year, int employeeId)
        {
            var employee = await _context.Employee
                .Include(e => e.GeneralSettings)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            decimal bonusesByHours = 0;
            decimal deductionsByHours = 0;
            int attendedDays = 0;
            int absentDays = 0;
            var dailyDetails = new List<DailyAttendanceDetail>();

            var holidays = await _context.Holidays
                .Where(h => h.Date.Month == month && h.Date.Year == year)
                .Select(h => h.Date)
                .ToListAsync();

            var weeklyHolidays = employee.GeneralSettings.WeeklyHolidayList;

            int daysInMonth = DateTime.DaysInMonth(year, month);
            for (int day = 1; day <= daysInMonth; day++)
            {
                var currentDate = new DateTime(year, month, day);
                var attendanceRecord = _context.AttendanceTables
                    .FirstOrDefault(a => a.EmployeeId == employee.Id && a.Date.Date == currentDate.Date);

                bool isWeeklyHoliday = weeklyHolidays.Contains((Day)currentDate.DayOfWeek);
                bool isHoliday = holidays.Contains(currentDate);

                var detail = new DailyAttendanceDetail
                {
                    Date = currentDate,
                    IsHoliday = isHoliday,
                    IsWeeklyHoliday = isWeeklyHoliday,
                    Attended = attendanceRecord != null
                };

                if (attendanceRecord != null)
                {
                    attendedDays++;
                    var workedHours = (attendanceRecord.DepartureTime - attendanceRecord.AttendanceTime)?.TotalHours ?? 0;
                    detail.ArrivalTime = attendanceRecord.AttendanceTime;
                    detail.DepartureTime = attendanceRecord.DepartureTime;

                    if (attendanceRecord.AttendanceTime > employee.AttendanceTime)
                    {
                        var lateHours = (attendanceRecord.AttendanceTime - employee.AttendanceTime).TotalHours;
                        deductionsByHours += (decimal)lateHours * employee.GeneralSettings.rivalPerHour;
                        detail.Deduction = (decimal)lateHours * employee.GeneralSettings.rivalPerHour;
                    }
                    else if (attendanceRecord.AttendanceTime < employee.AttendanceTime)
                    {
                        var earlyHours = (employee.AttendanceTime - attendanceRecord.AttendanceTime).TotalHours;
                        bonusesByHours += (decimal)earlyHours * employee.GeneralSettings.bonusPerHoure;
                        detail.Bonus = (decimal)earlyHours * employee.GeneralSettings.bonusPerHoure;
                    }

                    if (attendanceRecord.DepartureTime < employee.DepartureTime)
                    {
                        var earlyLeaveHours = (employee.DepartureTime - attendanceRecord.DepartureTime)?.TotalHours;
                        deductionsByHours += (decimal)earlyLeaveHours * employee.GeneralSettings.rivalPerHour;
                        detail.Deduction += (decimal)earlyLeaveHours * employee.GeneralSettings.rivalPerHour;
                    }
                    else if (attendanceRecord.DepartureTime > employee.DepartureTime)
                    {
                        var lateLeaveHours = (attendanceRecord.DepartureTime - employee.DepartureTime)?.TotalHours;
                        bonusesByHours += (decimal)lateLeaveHours * employee.GeneralSettings.bonusPerHoure;
                        detail.Bonus += (decimal)lateLeaveHours * employee.GeneralSettings.bonusPerHoure;
                    }
                }
                else
                {
                    detail.Attended = false;
                    if (!(isHoliday || isWeeklyHoliday))
                    {
                        absentDays++;

                        var missedDayHours = (employee.DepartureTime - employee.AttendanceTime).TotalHours;
                        var missedDayDeduction = (decimal)missedDayHours * employee.GeneralSettings.rivalPerHour;
                        deductionsByHours += missedDayDeduction;
                        detail.Deduction = missedDayDeduction;
                    }
                }

                dailyDetails.Add(detail);
            }

            decimal HourPrice = (employee.Salary / 30) / (employee.DepartureTime - employee.AttendanceTime).Hours;

            var netSalary = employee.Salary + HourPrice * (bonusesByHours - deductionsByHours);

            if (absentDays == decimal.Zero && attendedDays == decimal.Zero)
            {
                HourPrice = 0;
                netSalary = employee.Salary;
            }

            var report = new EmployeeMonthlyReportViewModel
            {
                EmployeeName = employee.EmployeeName,
                BasicSalary = employee.Salary,
                AttendedDays = attendedDays,
                AbsentDays = absentDays,
                BonusesByHours = bonusesByHours,
                DeductionsByHours = deductionsByHours,
                NetSalary = netSalary,
                DailyDetails = dailyDetails,
                HourPrice = HourPrice,
                BonusesByMoney = bonusesByHours * HourPrice,
                DeductionsByMoney = deductionsByHours * HourPrice
            };

            return View(report);
        }


    }
}
