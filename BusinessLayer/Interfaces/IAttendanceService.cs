using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IAttendanceService
    {
        Task<List<AttendanceTable>> GetAttendanceAsync(int? employeeId, DateTime? startDate, DateTime? endDate);
        Task<AttendanceTable> GetAttendanceByIdAsync(int id);
        Task CreateAttendanceAsync(AttendanceTable attendance);
        Task UpdateAttendanceAsync(AttendanceTable attendance);
        Task DeleteAttendanceAsync(int id);
    }
}
