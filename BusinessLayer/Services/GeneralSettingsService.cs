using BusinessLayer.Interfaces;
using DataLayer.Data;
using DataLayer.Entities;
using DataLayer.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services
{
    public class GeneralSettingsService : IGeneralSettingsService
    {
        private readonly ApplicationDbContext _context;

        public GeneralSettingsService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<EmployeeGeneralSettingsViewModel>> GetEmployeesWithSettingsAsync()
        {
            var employeesWithSettings = await _context.Employee
                .Include(e => e.GeneralSettings)
                .ToListAsync();

            return employeesWithSettings.Select(e => new EmployeeGeneralSettingsViewModel
            {
                EmployeeId = e.Id,
                EmployeeName = e.EmployeeName,
                Image = e.image,
                BonusPerHour = e.GeneralSettings?.bonusPerHoure ?? 0, // Default to 0 if null
                RivalPerHour = e.GeneralSettings?.rivalPerHour ?? 0, // Default to 0 if null
                WeeklyHolidays = e.GeneralSettings?.WeeklyHolidays ?? "Not Set" // Default to "Not Set" if null
            }).ToList();
        }


        public async Task<GeneralSettings> GetSettingsByEmployeeIdAsync(int id)
        {
            var settings = await _context.GeneralSettings.Include(gs => gs.Employee).FirstOrDefaultAsync(gs => gs.EmployeeId == id);
           
            
            if (settings == null)
            {
                settings = new GeneralSettings
                {
                    EmployeeId = id,
                    WeeklyHolidays = "Saturday,Sunday"
                };

                _context.GeneralSettings.Add(settings);
                await _context.SaveChangesAsync();
            }

            return settings;
        }

        public async Task UpdateSettingsAsync(GeneralSettings model)
        {

            var existingSettings = await _context.GeneralSettings
                .FirstOrDefaultAsync(s => s.EmployeeId == model.EmployeeId);

            existingSettings.bonusPerHoure = model.bonusPerHoure;
            existingSettings.rivalPerHour = model.rivalPerHour;
            existingSettings.WeeklyHolidays = model.WeeklyHolidays;

            await _context.SaveChangesAsync();
        }




        public async Task<EmployeeDetailsViewModel> GetEmployeeDetailsAsync(int id)
        {
            var employee = await _context.Employee
                .Include(e => e.GeneralSettings)
                .FirstOrDefaultAsync(e => e.Id == id);
               Console.WriteLine(employee.Id);

            if (employee.GeneralSettings == null)
            {
                return new EmployeeDetailsViewModel
                {
                    EmployeeId = employee.Id,
                    EmployeeName = employee.EmployeeName,
                    Image = employee.image,
                    BonusPerHour = 0,
                    RivalPerHour = 0,
                    WeeklyHolidays = "",
                    SettingsExists = false
                };
            }

            return new EmployeeDetailsViewModel
            {
                EmployeeId = employee.Id,
                EmployeeName = employee.EmployeeName,
                Image = employee.image,
                BonusPerHour = employee.GeneralSettings.bonusPerHoure,
                RivalPerHour = employee.GeneralSettings.rivalPerHour,
                WeeklyHolidays = employee.GeneralSettings.WeeklyHolidays,
                SettingsExists = true
            };
        }

        public async Task<List<Employee>> GetEmployeesWithoutSettingsAsync()
        {
            return await _context.Employee
                .Where(e => e.GeneralSettings == null)
                .ToListAsync();
        }


        public async Task CreateGeneralSettingsAsync(GeneralSettingsViewModel model)
        {

            var settings = new GeneralSettings
            {
                EmployeeId = model.EmployeeId,
                bonusPerHoure = model.BonusPerHour,
                rivalPerHour = model.RivalPerHour,
                WeeklyHolidays = string.Join(",", model.SelectedDays)
            };

            _context.GeneralSettings.Add(settings);
            await _context.SaveChangesAsync();
        }


    }
}
