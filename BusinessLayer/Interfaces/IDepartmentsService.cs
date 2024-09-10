using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IDepartmentsService
    {
        List<Department> GetDepartments();

        Task<Department> GetDepartmentById(int id);

        Task<Department> AddDepartment(Department department);

        Task<Department> UpdateDepartment(Department department);

        Task<Department> DeleteDepartment(int id);

        Task<Department> GetDepartmentByName(string name);



    }
}
