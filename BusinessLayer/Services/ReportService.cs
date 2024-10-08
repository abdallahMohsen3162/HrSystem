﻿using DataLayer.Entities;
using DataLayer.ViewModels;
using DataLayer.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;
using NuGet.Packaging;
using Humanizer;

namespace HrSystem.Services
{
    public class ReportService : IReportService
    {
        private readonly ApplicationDbContext _context;

        public ReportService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeMonthlyReportViewModel>> GenerateMultiReportAsync(int month, int year, int departmentId)
        {



            var employees = await _context.Employee
                .Where(e => e.DepartmentId == departmentId || departmentId == -1)
                .Include(e => e.GeneralSettings)
                .ToListAsync();

            var holidays = await _context.Holidays
                .Where(h => h.Date.Month == month && h.Date.Year == year)
                .Select(h => h.Date)
                .ToListAsync();

            var monthlyReports = new List<EmployeeMonthlyReportViewModel>();

            foreach (var employee in employees)
            {
                Employee emp = employee;
                
                var report = GenerateEmployeeMonthlyReport(emp, month, year, holidays);
                if (report != null)
                {
                    monthlyReports.Add(report);
                }
            }

            return monthlyReports;
        }

        public async Task<EmployeeMonthlyReportViewModel> GenerateEmployeeMonthlyReportAsync(int month, int year, int employeeId)
        {
            var employee = await _context.Employee
                .Include(e => e.GeneralSettings)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null)
            {
                return null;
            }

            var holidays = await _context.Holidays
                .Where(h => h.Date.Month == month && h.Date.Year == year)
                .Select(h => h.Date)
                .ToListAsync();

            return GenerateEmployeeMonthlyReport(employee, month, year, holidays);
        }


        public Dictionary<string, List<EmployeeMonthlyReportViewModel>> GenerateDepartmentReport(int month, int year, int departmentId = -1)
        {
            var employees = _context.Employee.Include(e => e.Department).Include(e => e.GeneralSettings)
                .Where(e => e.DepartmentId == departmentId || departmentId == -1)
                .ToList();

            Dictionary<string, List<EmployeeMonthlyReportViewModel>> mp = new Dictionary<string, List<EmployeeMonthlyReportViewModel>>();
            var holidays = _context.Holidays
                .Where(h => h.Date.Month == month && h.Date.Year == year)
                .Select(h => h.Date)
                .ToList();

            foreach (var employee in employees)
            {
                if (employee.Department != null)
                {

                    var report = GenerateEmployeeMonthlyReport(employee, month, year, holidays);

                    if (report != null)
                    {

                        var departmentName = employee.Department.DepartmentName;

                        if (!mp.ContainsKey(departmentName))
                        {
                            mp[departmentName] = new List<EmployeeMonthlyReportViewModel>();
                        }
                        mp[departmentName].Add(report);

                    }
                }
            }
            return mp;
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
                        EmployeeId = employee.Id,
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

                var privateHolidays = _context.PrivateHolidays
                    .Where(ph => ph.EmployeeId == employee.Id && ph.HolidayDate >= startDate && ph.HolidayDate <= endDate)
                    .Select(ph => ph.HolidayDate.Date)
                    .ToHashSet();

                foreach (var holiday in holidays)
                {
                    privateHolidays.Add(holiday);
                }

                var attendanceRecords = _context.AttendanceTables
                    .Where(a => a.EmployeeId == employee.Id && a.Date >= startDate && a.Date <= endDate)
                    .ToHashSet();

                var weeklyHolidayInts = employee.GeneralSettings.WeeklyHolidayList
                    .Select(d => (int)d)
                    .ToHashSet();

                int attendedDays = attendanceRecords
                    .Where(a => !weeklyHolidayInts.Contains((int)a.Date.DayOfWeek) && !privateHolidays.Contains(a.Date.Date) && !holidays.Contains(a.Date.Date))
                    .Count();

                int totalWorkingDays = Enumerable.Range(0, (endDate - startDate).Days + 1)
                    .Select(d => startDate.AddDays(d))
                    .Count(date => !weeklyHolidayInts.Contains((int)date.DayOfWeek) && !privateHolidays.Contains(date.Date) && !holidays.Contains(date.Date));

                int absentDays = totalWorkingDays - attendedDays;

                decimal priceofabsentDays = HourPrice * absentDays * (decimal)(employee.DepartureTime - employee.AttendanceTime).TotalHours;

                decimal totalDiscountsMoney = priceofabsentDays + DiscountHours * HourPrice * EmployeediscountHour;
                decimal totalBonusMoney = BonusHours * HourPrice * EmployeebonusHour;

                decimal netSalary = SalaryTillNow + (totalBonusMoney - totalDiscountsMoney);
                netSalary *= (attendanceRecords.Count > 0 ? 1 : 0);

                return new EmployeeMonthlyReportViewModel
                {
                    EmployeeId = employee.Id,
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
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return new EmployeeMonthlyReportViewModel
                {
                    EmployeeId = employee.Id,
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



    }
 


}
