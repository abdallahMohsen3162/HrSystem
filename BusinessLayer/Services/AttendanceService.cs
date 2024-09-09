using DataLayer.Entities;
using DataLayer.Data;
using DataLayer.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;

namespace BusinessLogic.Services
{
    public class AttendanceService : IAttendanceService
    {
        private readonly ApplicationDbContext _context;

        public AttendanceService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<AttendanceTable>> GetAttendanceRecords(int? employeeId, DateTime? startDate, DateTime? endDate)
        {
            var attendanceQuery = _context.AttendanceTables.Include(a => a.Employee).AsQueryable();

            if (employeeId.HasValue)
            {
                attendanceQuery = attendanceQuery.Where(a => a.EmployeeId == employeeId.Value);
            }

            if (startDate.HasValue)
            {
                attendanceQuery = attendanceQuery.Where(a => a.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                attendanceQuery = attendanceQuery.Where(a => a.Date <= endDate.Value);
            }

            return await attendanceQuery.ToListAsync();
        }

        public async Task<List<Employee>> GetEmployeesAsync()
        {
            return await _context.Employee.ToListAsync();
        }

        public async Task<AttendanceTable> GetAttendanceById(int id)
        {
            return await _context.AttendanceTables.FindAsync(id);
        }

        public async Task CreateAttendance(AttendanceTable attendance)
        {
            var employee = await _context.Employee.FindAsync(attendance.EmployeeId);
            if (employee != null)
            {
                attendance.Discount = 0;
                attendance.Bonus = 0;
                attendance.Discount += Math.Max((decimal)(employee.DepartureTime - attendance.DepartureTime)?.TotalHours, 0);
                attendance.Discount += Math.Max((decimal)(attendance.AttendanceTime - employee.AttendanceTime).TotalHours, 0);
                attendance.Bonus += Math.Max((decimal)(employee.DepartureTime - attendance.DepartureTime)?.TotalHours, 0);

                _context.Add(attendance);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateAttendance(AttendanceTable attendance)
        {
            var employee = await _context.Employee.FindAsync(attendance.EmployeeId);
            if (employee != null)
            {
                attendance.Discount = 0;
                attendance.Bonus = 0;
                attendance.Discount += Math.Max((decimal)(employee.DepartureTime - attendance.DepartureTime)?.TotalHours, 0);
                attendance.Discount += Math.Max((decimal)(attendance.AttendanceTime - employee.AttendanceTime).TotalHours, 0);
                attendance.Bonus += Math.Max((decimal)(attendance.DepartureTime - employee.DepartureTime)?.TotalHours, 0);

                _context.Update(attendance);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAttendance(int id)
        {
            var attendance = await _context.AttendanceTables.FindAsync(id);
            _context.AttendanceTables.Remove(attendance);
            await _context.SaveChangesAsync();
        }

        public async Task<List<AttendanceTable>> GetPermissions(int? employeeId, DateTime? startDate, DateTime? endDate)
        {
            var attendanceQuery = _context.AttendanceTables.Where(a => a.EarlyTime > 0).Include(a => a.Employee).AsQueryable();

            if (employeeId.HasValue)
            {
                attendanceQuery = attendanceQuery.Where(a => a.EmployeeId == employeeId.Value);
            }

            if (startDate.HasValue)
            {
                attendanceQuery = attendanceQuery.Where(a => a.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                attendanceQuery = attendanceQuery.Where(a => a.Date <= endDate.Value);
            }

            return await attendanceQuery.ToListAsync();
        }

        public async Task SaveEarlyTime(SaveEarlyTimeViewModel model)
        {
            var attendanceRecord = await _context.AttendanceTables
                .FirstOrDefaultAsync(a => a.EmployeeId == model.EmployeeId && a.Date == model.Date);
            if (attendanceRecord == null)
            {
                return;
            }
            attendanceRecord.EarlyTime = model.Hours;
            await _context.SaveChangesAsync();
        }
    }
}
