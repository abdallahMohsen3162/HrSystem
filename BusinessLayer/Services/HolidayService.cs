using BusinessLayer.Interfaces;
using DataLayer.Data;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class HolidayService : IHolidayService
    {
        private readonly ApplicationDbContext _context;

        public HolidayService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Holiday>> GetAllHolidaysAsync()
        {
            return await _context.Holidays.ToListAsync();
        }

        public async Task<Holiday> GetHolidayByIdAsync(int id)
        {
            return await _context.Holidays.FindAsync(id);
        }

        public async Task AddHolidayAsync(Holiday holiday)
        {
            _context.Add(holiday);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateHolidayAsync(Holiday holiday)
        {
            _context.Update(holiday);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteHolidayAsync(int id)
        {
            var holiday = await _context.Holidays.FindAsync(id);
            if (holiday != null)
            {
                _context.Holidays.Remove(holiday);
                await _context.SaveChangesAsync();
            }
        }

        public bool HolidayExists(int id)
        {
            return _context.Holidays.Any(e => e.Id == id);
        }

        public List<string> GetHolidayDaysOfWeek(List<Holiday> holidays)
        {
            List<string> days = new List<string>();
            foreach (var holiday in holidays)
            {
                DateTime date = new DateTime(holiday.Date.Year, holiday.Date.Month, holiday.Date.Day);
                string str = date.DayOfWeek.ToString();
                days.Add(str);
            }
            return days;
        }
    }
}
