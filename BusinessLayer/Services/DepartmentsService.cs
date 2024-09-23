using BusinessLayer.Interfaces;
using DataLayer.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Data;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services
{
    public class DepartmentsService : IDepartmentsService
    {
        private readonly ApplicationDbContext _context;

        public DepartmentsService(ApplicationDbContext context)
        {
            _context = context;
        }


        public List<Department> GetDepartments()
        {
            return _context.Departments.ToList();
        }


        public async Task<Department> GetDepartmentById(int id)
        {
            return await _context.Departments
                                 .FirstOrDefaultAsync(dept => dept.Id == id);
        }


        public async Task<Department> AddDepartment(Department department)
        {
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
            return department;
        }


        public async Task<Department> UpdateDepartment(Department department)
        {
            var existingDepartment = await _context.Departments.FindAsync(department.Id);

            if (existingDepartment != null)
            {
                existingDepartment.DepartmentName = department.DepartmentName;

                _context.Departments.Update(existingDepartment);
                await _context.SaveChangesAsync();
            }

            return existingDepartment;
        }

        public Department DeleteDepartment(int id)
        {
            try
            {
                var department =  _context.Departments.Find(id);

                if (department == null)
                {
                    Console.WriteLine("Department not found");
                    return null;
                }

                Console.WriteLine(department.DepartmentName);

                var employees =  _context.Employee
                    .Where(e => e.DepartmentId == id)
                    .ToList();

                foreach (var employee in employees)
                {
                    employee.DepartmentId = null;
                    _context.Employee.Update(employee);
                }

                 _context.SaveChanges(); 

                _context.Departments.Remove(department);

                 _context.SaveChanges(); 

                return department;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }



        public async Task<Department> GetDepartmentByName(string name)
        {
            return await _context.Departments.FirstOrDefaultAsync(dept => dept.DepartmentName == name);
        }
    }
}
