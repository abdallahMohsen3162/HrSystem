using AutoMapper;
using BusinessLayer.Interfaces;
using DataLayer.Data;
using DataLayer.dto.Departments;
using DataLayer.dto.Employee;
using DataLayer.Entities;
using DataLayer.Validation.Fluent_validation;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace HrSystem.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController : ControllerBase
    {

        private readonly IDepartmentsService departmentsService;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;

        public DepartmentController(IDepartmentsService departmentsService, IMapper mapper, ApplicationDbContext context)
        {
            this.departmentsService = departmentsService;
            _mapper = mapper;
            _context = context; 
        }
        [HttpGet]
        public IActionResult getAllDepartments()
        {
            try
            {   

                List<Department> departments = departmentsService.GetDepartments();

                List<DepartmentDto> departmentDtos = _mapper.Map<List<DepartmentDto>>(departments);

                return Ok(departmentDtos);

            }catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async  Task<IActionResult> getDepartmentById(int id)
        {
            try
            {
                Department department = departmentsService.GetDepartmentById(id).Result;

                DepartmentDto departmentDto = _mapper.Map<DepartmentDto>(department);

                return Ok(departmentDto);

            }catch(Exception ex)
            {   
                return BadRequest(ex.Message);
            }

        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] Department department)
        {
            try
            {

                AddDepartmentValidator validate = new AddDepartmentValidator(_context);
                var res  = validate.Validate(department);

                if(!res.IsValid)
                {
                    return BadRequest(res.Errors);
                }
                await departmentsService.AddDepartment(department);

                return Ok(department);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        [HttpPatch("{id}")]

        public async Task<IActionResult> Update(int id, [FromForm] Department department)
        {
            try
            {

                AddDepartmentValidator validate = new AddDepartmentValidator(_context);
                var res  = validate.Validate(department);

                if(!res.IsValid)
                {
                    return BadRequest(res.Errors);
                }

                await departmentsService.UpdateDepartment(department);

                return Ok(department);

            }catch(Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        [HttpDelete("{id}")]

        public IActionResult Delete(int id)
        {
            try
             {

                departmentsService.DeleteDepartment(id);

                return Ok();

            }catch(Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }




    }
}
