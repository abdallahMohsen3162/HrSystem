using DataLayer.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IHolidayService
    {
        Task<List<Holiday>> GetAllHolidaysAsync();
        Task<Holiday> GetHolidayByIdAsync(int id);
        Task AddHolidayAsync(Holiday holiday);
        Task UpdateHolidayAsync(Holiday holiday);
        Task DeleteHolidayAsync(int id);
        bool HolidayExists(int id);
        List<string> GetHolidayDaysOfWeek(List<Holiday> holidays);
    }
}
