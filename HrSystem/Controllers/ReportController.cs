using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.ViewModels;
using DataLayer.Entities;
using DataLayer.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace HrSystem.Controllers
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
            if (month == -1 || year == -1)
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
                var report = GenerateEmployeeMonthlyReport(employee, month, year, holidays);
                if (report != null)
                {
                    monthlyReports.Add(report);
                }
            }

            return View(monthlyReports);
        }

        [HttpGet]
        public async Task<IActionResult> EmployeeMonthlyReport(int month, int year, int employeeId)
        {
            var employee = await _context.Employee
                .Include(e => e.GeneralSettings)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            var holidays = await _context.Holidays
                .Where(h => h.Date.Month == month && h.Date.Year == year)
                .Select(h => h.Date)
                .ToListAsync();

            var report = GenerateEmployeeMonthlyReport(employee, month, year, holidays);

            return View(report);
        }

        private EmployeeMonthlyReportViewModel GenerateEmployeeMonthlyReport(Employee employee, int month, int year, List<DateTime> holidays)
        {
            try
            {


                var startDate = new DateTime(year, month, 1);
                var endDate = startDate.AddMonths(1).AddDays(-1);
                if (employee.JoinDate > startDate)
                {
                    startDate = employee.JoinDate;
                }

                if (endDate > DateTime.Now)
                {
                    endDate = DateTime.Now;
                }

                if (startDate < employee.JoinDate && employee.JoinDate < endDate)
                {
                    return new EmployeeMonthlyReportViewModel
                    {
                        EmployeeName = employee.EmployeeName,
                        BasicSalary = 0,
                        AttendedDays = 0,
                        AbsentDays = 0,
                        BonusesByHours = 0,
                        DeductionsByHours = 0,
                        DeductionsByMoney = 0,
                        BonusesByMoney = 0,
                        NetSalary = 0,
                        HourPrice = 0,
                    };
                }
                decimal EmployeebonusHour = employee.GeneralSettings.bonusPerHoure;

                decimal EmployeediscountHour = employee.GeneralSettings.rivalPerHour;

                decimal HourPrice = (decimal)(employee.Salary / 30) / (decimal)(employee.DepartureTime - employee.AttendanceTime).TotalHours;

                int daysInMonth = (endDate - startDate).Days;

                decimal SalaryPercent = (decimal)daysInMonth / 30;

                decimal SalaryTillNow = (decimal)employee.Salary * SalaryPercent;

                decimal DiscountHours = _context.AttendanceTables
                    .Where(a => a.EmployeeId == employee.Id && a.Date >= startDate && a.Date <= endDate)
                    .Sum(a => a.Discount) ?? 0m;

                decimal BonusHours = _context.AttendanceTables
                    .Where(a => a.EmployeeId == employee.Id && a.Date >= startDate && a.Date <= endDate)
                    .Sum(a => a.Bonus) ?? 0m;


                var attendanceRecords = _context.AttendanceTables
                    .Where(a => a.EmployeeId == employee.Id && a.Date >= startDate && a.Date <= endDate)
                    .ToList();


                var weeklyHolidayInts = employee.GeneralSettings.WeeklyHolidayList
                    .Select(d => (int)d)
                    .ToList();


                int attendedDays = attendanceRecords
                    .Where(a => !weeklyHolidayInts.Contains((int)a.Date.DayOfWeek) && !holidays.Contains(a.Date.Date))
                    .Count();



                int totalWorkingDays = Enumerable.Range(0, (endDate - startDate).Days)
                   .Select(d => startDate.AddDays(d))
                   .Count(date => !weeklyHolidayInts.Contains((int)date.DayOfWeek) && !holidays.Contains(date.Date));

                int absentDays = totalWorkingDays - attendedDays;
                decimal priceofabsentDays = HourPrice * absentDays * (decimal)(employee.DepartureTime - employee.AttendanceTime).TotalHours;

                decimal totalDiscountsMoney = priceofabsentDays + DiscountHours * HourPrice * EmployeediscountHour;
                decimal totalBonusMoney = BonusHours * HourPrice * EmployeebonusHour;


                decimal netSalary = SalaryTillNow + (totalBonusMoney - totalDiscountsMoney);

                return new EmployeeMonthlyReportViewModel
                {
                    EmployeeName = employee.EmployeeName,
                    BasicSalary = SalaryTillNow,
                    AttendedDays = attendedDays,
                    AbsentDays = absentDays,
                    BonusesByHours = BonusHours * EmployeebonusHour,
                    DeductionsByHours = DiscountHours * EmployeediscountHour,
                    DeductionsByMoney = totalDiscountsMoney,
                    BonusesByMoney = totalBonusMoney,
                    NetSalary = netSalary,
                    HourPrice = HourPrice,
                };
            }catch
            {
                return new EmployeeMonthlyReportViewModel
                {
                    EmployeeName = employee.EmployeeName,
                    BasicSalary = 0,
                    AttendedDays = 0,
                    AbsentDays = 0,
                    BonusesByHours = 0,
                    DeductionsByHours = 0,
                    DeductionsByMoney = 0,
                    BonusesByMoney = 0,
                    NetSalary = 0,
                    HourPrice = 0,
                };
            }

        }

        

        //private EmployeeMonthlyReportViewModel GenerateEmployeeMonthlyReport(Employee employee, int month, int year, List<DateTime> holidays)
        //{
        //    decimal bonusesByHours = 0;
        //    decimal deductionsByHours = 0;
        //    int attendedDays = 0;
        //    int absentDays = 0;
        //    var dailyDetails = new List<DailyAttendanceDetail>();

        //    var weeklyHolidays = employee.GeneralSettings.WeeklyHolidayList;
        //    int daysInMonth = DateTime.DaysInMonth(year, month);

        //    decimal salaryPercent = 0;
        //    for (int day = 1; day <= daysInMonth; day++)
        //    {
        //        var currentDate = new DateTime(year, month, day);
        //        if (currentDate >= employee.JoinDate.Date && currentDate <= DateTime.Now)
        //        {
        //            salaryPercent++;
        //        }
        //    }
        //    salaryPercent /= daysInMonth;

        //    for (int day = 1; day <= daysInMonth; day++)
        //    {
        //        var currentDate = new DateTime(year, month, day);
        //        if (currentDate < employee.JoinDate || currentDate > DateTime.Now)
        //        {
        //            continue;
        //        }

        //        var attendanceRecord = _context.AttendanceTables
        //            .FirstOrDefault(a => a.EmployeeId == employee.Id && a.Date.Date == currentDate.Date);

        //        bool isWeeklyHoliday = weeklyHolidays.Contains((Day)currentDate.DayOfWeek);
        //        bool isHoliday = holidays.Contains(currentDate);

        //        var detail = new DailyAttendanceDetail
        //        {
        //            Date = currentDate,
        //            IsHoliday = isHoliday,
        //            IsWeeklyHoliday = isWeeklyHoliday,
        //            Attended = attendanceRecord != null
        //        };

        //        if (attendanceRecord != null)
        //        {
        //            attendedDays++;
        //            var workedHours = (attendanceRecord.DepartureTime - attendanceRecord.AttendanceTime)?.TotalHours ?? 0;
        //            detail.ArrivalTime = attendanceRecord.AttendanceTime;
        //            detail.DepartureTime = attendanceRecord.DepartureTime;

        //            if (attendanceRecord.AttendanceTime > employee.AttendanceTime)
        //            {
        //                var lateHours = (attendanceRecord.AttendanceTime - employee.AttendanceTime).TotalHours;
        //                deductionsByHours += (decimal)lateHours * employee.GeneralSettings.rivalPerHour;
        //                detail.Deduction = (decimal)lateHours * employee.GeneralSettings.rivalPerHour;
        //            }
        //            else if (attendanceRecord.AttendanceTime < employee.AttendanceTime)
        //            {
        //                var earlyHours = (employee.AttendanceTime - attendanceRecord.AttendanceTime).TotalHours;
        //                bonusesByHours += (decimal)earlyHours * employee.GeneralSettings.bonusPerHoure;
        //                detail.Bonus = (decimal)earlyHours * employee.GeneralSettings.bonusPerHoure;
        //            }

        //            if (attendanceRecord.DepartureTime < employee.DepartureTime)
        //            {
        //                var earlyLeaveHours = (employee.DepartureTime - attendanceRecord.DepartureTime)?.TotalHours;
        //                deductionsByHours += (decimal)earlyLeaveHours * employee.GeneralSettings.rivalPerHour;
        //                detail.Deduction += (decimal)earlyLeaveHours * employee.GeneralSettings.rivalPerHour;
        //            }
        //            else if (attendanceRecord.DepartureTime > employee.DepartureTime)
        //            {
        //                var lateLeaveHours = (attendanceRecord.DepartureTime - employee.DepartureTime)?.TotalHours;
        //                bonusesByHours += (decimal)lateLeaveHours * employee.GeneralSettings.bonusPerHoure;
        //                detail.Bonus += (decimal)lateLeaveHours * employee.GeneralSettings.bonusPerHoure;
        //            }
        //        }
        //        else
        //        {
        //            detail.Attended = false;
        //            if (!(isHoliday || isWeeklyHoliday))
        //            {
        //                absentDays++;
        //                var missedDayHours = (employee.DepartureTime - employee.AttendanceTime).TotalHours;
        //                var missedDayDeduction = (decimal)missedDayHours * employee.GeneralSettings.rivalPerHour;
        //                deductionsByHours += missedDayDeduction;
        //                detail.Deduction = missedDayDeduction;
        //            }
        //        }

        //        dailyDetails.Add(detail);
        //    }


        //    decimal actualSalary = employee.Salary * salaryPercent;
        //    decimal hourPrice = (actualSalary / 30) / (employee.DepartureTime - employee.AttendanceTime).Hours;
        //    var netSalary = actualSalary + hourPrice * (bonusesByHours - deductionsByHours);


        //    if (absentDays == decimal.Zero && attendedDays == decimal.Zero)
        //    {
        //        hourPrice = 0;
        //        netSalary = actualSalary;
        //    }
        //    ViewBag.DaysInMonth = salaryPercent * daysInMonth;
        //    ViewBag.Month = daysInMonth;
        //    return new EmployeeMonthlyReportViewModel
        //    {
        //        EmployeeName = employee.EmployeeName,
        //        BasicSalary = actualSalary,
        //        AttendedDays = attendedDays,
        //        AbsentDays = absentDays,
        //        BonusesByHours = bonusesByHours,
        //        DeductionsByHours = deductionsByHours,
        //        NetSalary = netSalary,
        //        DailyDetails = dailyDetails,
        //        HourPrice = hourPrice,
        //        BonusesByMoney = bonusesByHours * hourPrice,
        //        DeductionsByMoney = deductionsByHours * hourPrice
        //    };
        //}
    }
}
