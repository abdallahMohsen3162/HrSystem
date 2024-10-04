using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;

namespace Api
{
    public class Employee
    {
        private readonly IEmployeeService _employeeService;
        public Employee(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

    }
}
