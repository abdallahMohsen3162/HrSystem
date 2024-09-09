using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IAttendanceService
    {
        Task<List<AttendanceTable>> GetAttendanceRecords(int? employeeId, DateTime? startDate, DateTime? endDate);
        Task<List<Employee>> GetEmployeesAsync();
        Task<AttendanceTable> GetAttendanceById(int id);
        Task CreateAttendance(AttendanceTable attendance);
        Task UpdateAttendance(AttendanceTable attendance);
        Task DeleteAttendance(int id);
        Task<List<AttendanceTable>> GetPermissions(int? employeeId, DateTime? startDate, DateTime? endDate);
        Task SaveEarlyTime(SaveEarlyTimeViewModel model);
    }
}
