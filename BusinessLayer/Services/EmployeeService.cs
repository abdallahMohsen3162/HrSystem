using BusinessLayer.FileManagement;
using BusinessLayer.Interfaces;
using DataLayer.ViewModels;
using DataLayer.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using NuGet.Configuration;
namespace BusinessLayer.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ApplicationDbContext _context;
        private readonly FileManager _fileManager;

        public EmployeeService(ApplicationDbContext context)
        {
            _context = context;
            _fileManager = new FileManager("wwwroot");
        }

        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            return await _context.Employee.Include(e => e.Department).Include(e => e.GeneralSettings).ToListAsync();
        }

        public async Task<Employee> GetEmployeeByIdAsync(int id)
        {
            return await _context.Employee.FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task CreateEmployeeAsync(CreateEmployeeViewModel employee)
        {
            Console.WriteLine(employee.DepartmentId);
            string imageUrl = await _fileManager.SaveFileAsync(employee.ProfileImage, "Employees/Images");
            var emp = new Employee
            {
                EmployeeName = employee.EmployeeName,
                Address = employee.Address,
                PhoneNumber = employee.PhoneNumber,
                Gender = employee.Gender,
                Nationality = employee.Nationality,
                DateOfBirth = employee.DateOfBirth,
                NationalId = employee.NationalId,
                JoinDate = employee.JoinDate,
                Salary = employee.Salary,
                AttendanceTime = employee.AttendanceTime,
                DepartureTime = employee.DepartureTime,
                image = imageUrl,
                DepartmentId = employee.DepartmentId,
                
            };


            _context.Add(emp);
            await _context.SaveChangesAsync();

            //var settings = new GeneralSettings
            //{
            //    EmployeeId = emp.Id,
            //    WeeklyHolidays = "Saturday,Sunday"
            //};

            //_context.GeneralSettings.Add(settings);
            //await _context.SaveChangesAsync();

        }

        public async Task UpdateEmployeeAsync(int id, EditeEmployeeViewModel employee)
        {

            var employeeEntity = await _context.Employee.FindAsync(id);
            if (employeeEntity == null)
            {
                throw new KeyNotFoundException("Employee not found");
            }

            if (employee.ProfileImage != null)
            {
                if (!string.IsNullOrEmpty(employeeEntity.image))
                {
                    _fileManager.DeleteFile(("/" + employeeEntity.image).TrimStart('/'));
                }
                string imageUrl = await _fileManager.SaveFileAsync(employee.ProfileImage, "Employees/Images");
                employeeEntity.image = imageUrl;
            }

            employeeEntity.EmployeeName = employee.EmployeeName;
            employeeEntity.Address = employee.Address;
            employeeEntity.PhoneNumber = employee.PhoneNumber;
            employeeEntity.Gender = employee.Gender;
            employeeEntity.Nationality = employee.Nationality;
            employeeEntity.DateOfBirth = employee.DateOfBirth;
            employeeEntity.NationalId = employee.NationalId;
            employeeEntity.JoinDate = employee.JoinDate;
            employeeEntity.Salary = employee.Salary;
            employeeEntity.AttendanceTime = employee.AttendanceTime;
            employeeEntity.DepartureTime = employee.DepartureTime;
            employeeEntity.DepartmentId = employee.DepartmentId;

            _context.Update(employeeEntity);
            await _context.SaveChangesAsync();
        

        }

        public async Task DeleteEmployeeAsync(int id)
        {
            var employee = await _context.Employee.FindAsync(id);
            if (employee != null)
            {
                _context.Employee.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }

        public bool EmployeeExists(int id)
        {
            return _context.Employee.Any(e => e.Id == id);
        }
    }
}
