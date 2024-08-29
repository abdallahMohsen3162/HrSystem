using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;
namespace BusinessLayer.Interfaces
{
    public interface IGeneralSettingsService
    {
        Task<List<EmployeeGeneralSettingsViewModel>> GetEmployeesWithSettingsAsync();
        Task<GeneralSettings> GetSettingsByEmployeeIdAsync(int id);
        Task UpdateSettingsAsync(GeneralSettings model);
        Task<EmployeeDetailsViewModel> GetEmployeeDetailsAsync(int id);

        Task<List<Employee>> GetEmployeesWithoutSettingsAsync();

        Task CreateGeneralSettingsAsync(GeneralSettingsViewModel model);
    }
}
