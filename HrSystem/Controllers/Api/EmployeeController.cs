using AutoMapper;
using BusinessLayer.Interfaces;
using DataLayer.dto.Employee;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;

namespace HrSystem.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService employeeService;
        private readonly IMapper _mapper;
        public EmployeeController(IEmployeeService employeeService, IMapper mapper)
        {
            this.employeeService = employeeService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                List<Employee> emp = employeeService.GetAllEmployeesAsync().Result;
                List<EmployeeDto> employees = _mapper.Map<List<EmployeeDto>>(emp);
                foreach (var employee in employees)
                {
                    Console.WriteLine(employee.EmployeeName);
                }
                return Ok(employees);
            }
            catch(Exception ex)
            {
               return BadRequest(ex.Message);
            }
        }
    }
}
