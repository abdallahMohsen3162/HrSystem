using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;
namespace BusinessLayer.Interfaces
{
    public interface IEmployeeService
    {
        Task<List<Employee>> GetAllEmployeesAsync();
        Task<Employee> GetEmployeeByIdAsync(int id);
        Task CreateEmployeeAsync(CreateEmployeeViewModel employee);
        Task UpdateEmployeeAsync(int id, EditeEmployeeViewModel employee);
        Task DeleteEmployeeAsync(int id);
        bool EmployeeExists(int id);
    }
}
