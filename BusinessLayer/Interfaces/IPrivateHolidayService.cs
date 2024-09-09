using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;
namespace BusinessLayer.Interfaces
{
    public interface IPrivateHolidayService
    {
        Task<List<PrivateHoliday>> GetAllPrivateHolidays();
        Task<List<Employee>> GetAllEmployees();
        Task<PrivateHoliday> GetPrivateHolidayById(int id);
        Task CreatePrivateHoliday(PrivateHoliday privateHoliday);
        Task UpdatePrivateHoliday(PrivateHoliday privateHoliday);
        Task DeletePrivateHoliday(int id);
        Task<bool> ValidateAttendanceRecord(int employeeId, DateTime holidayDate);
        Task<bool> ValidateHolidayDate(DateTime holidayDate, int employeeId, int? id);
        bool PrivateHolidayExists(int id);
    }
}
