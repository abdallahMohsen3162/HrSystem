using BusinessLayer.Interfaces;
using DataLayer.Data;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLogic.Services
{
    public class PrivateHolidayService : IPrivateHolidayService
    {
        private readonly ApplicationDbContext _context;

        public PrivateHolidayService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<PrivateHoliday>> GetAllPrivateHolidays()
        {
            return await _context.PrivateHolidays.Include(h => h.Employee).ToListAsync();
        }

        public async Task<List<Employee>> GetAllEmployees()
        {
            return await _context.Employee.ToListAsync();
        }

        public async Task<PrivateHoliday> GetPrivateHolidayById(int id)
        {
            return await _context.PrivateHolidays.FindAsync(id);
        }

        public async Task CreatePrivateHoliday(PrivateHoliday privateHoliday)
        {
            _context.Add(privateHoliday);
            await _context.SaveChangesAsync();
        }

        public async Task UpdatePrivateHoliday(PrivateHoliday privateHoliday)
        {
            _context.Update(privateHoliday);
            await _context.SaveChangesAsync();
        }

        public async Task DeletePrivateHoliday(int id)
        {
            var privateHoliday = await _context.PrivateHolidays.FindAsync(id);
            _context.PrivateHolidays.Remove(privateHoliday);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ValidateAttendanceRecord(int employeeId, DateTime holidayDate)
        {
            var attendanceRecord = await _context.AttendanceTables
                .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date == holidayDate);
            return attendanceRecord != null;
        }

        public async Task<bool> ValidateHolidayDate(DateTime holidayDate, int employeeId, int? id)
        {
            var dayExistsBefore = await _context.PrivateHolidays
                .Where(p => p.HolidayDate == holidayDate && p.EmployeeId == employeeId && p.Id != id)
                .FirstOrDefaultAsync();

            var officialHolidays = await _context.Holidays
                .Where(h => h.Date == holidayDate)
                .FirstOrDefaultAsync();

            if (dayExistsBefore != null || officialHolidays != null)
            {
                return false;
            }

            var employeeGeneralSettings = await _context.GeneralSettings
                .FirstOrDefaultAsync(gs => gs.EmployeeId == employeeId);

            if (employeeGeneralSettings != null)
            {
                var weeklyHolidays = employeeGeneralSettings.WeeklyHolidayList.ToList();
                var dayOfWeek = (int)holidayDate.DayOfWeek;

                if (weeklyHolidays.Contains((Day)dayOfWeek))
                {
                    return false;
                }
            }

            return true;
        }

        public bool PrivateHolidayExists(int id)
        {
            return _context.PrivateHolidays.Any(e => e.Id == id);
        }
    }
}
