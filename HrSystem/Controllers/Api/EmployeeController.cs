﻿using AutoMapper;
using BusinessLayer.Interfaces;
using DataLayer.Data;
using DataLayer.dto.Employee;
using DataLayer.Entities;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrSystem.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeService employeeService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        public EmployeeController(IEmployeeService employeeService, IMapper mapper, ApplicationDbContext context)
        {
            this.employeeService = employeeService;
            _mapper = mapper;
            _context = context;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Employee.Show)]
        [HttpGet("AllEmployees")]
        public ActionResult<List<EmployeeDto>> Index()
        {
            try
            {
                List<Employee> emp = employeeService.GetAllEmployeesAsync().Result;
                List<EmployeeDto> employees = _mapper.Map<List<EmployeeDto>>(emp);

                return employees;
            }
            catch(Exception ex)
            {
               return BadRequest(ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Employee.Show)]
        [HttpGet("employee/{id}")]
        public ActionResult<EmployeeDto> Get(int id)
        {
            try
            {
                Employee emp = employeeService.GetEmployeeByIdAsync(id).Result;
                EmployeeDto employee = _mapper.Map<EmployeeDto>(emp);
                return employee;
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Employee.Add)]
        [HttpPost("create")]
        public async Task<ActionResult<CreateEmployeeViewModel>> Post([FromForm] CreateEmployeeViewModel employee)
        {
            try
            {
                var validate = new CreateEmployeeViewModelValidator(this._context);
                var result = validate.Validate(employee);
                if (!result.IsValid)
                {
                    return BadRequest(result.Errors);
                }

                await employeeService.CreateEmployeeAsync(employee);

                return employee;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                return BadRequest(ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Employee.Delete)]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id) 
        {
            try
            {
                await employeeService.DeleteEmployeeAsync(id);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Employee.Edit)]
        [HttpPatch("edit/{id}")]
        //public async Task UpdateEmployeeAsync(int id, EditeEmployeeViewModel employee)
        public async Task<IActionResult> Update(int id, [FromForm] EditeEmployeeViewModel employee)
        {
            try
            {
                var validate = new CreateEmployeeViewModelValidator(this._context);
                await employeeService.UpdateEmployeeAsync(id, employee);
                return Ok();
            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }
}
