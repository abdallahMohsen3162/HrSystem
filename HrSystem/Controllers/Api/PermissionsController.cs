using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataLayer.Data;
using DataLayer.dto.Attendence;
using DataLayer.dto.Roles;
using DataLayer.dto.Users;
using DataLayer.Entities;
using DataLayer.Validation;
using DataLayer.Validation.Fluent_validation;
using DataLayer.ViewModels;
using Humanizer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace HrSystem.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendancesController : ControllerBase
    {
        private readonly IAttendanceService attendanceService;
        private readonly ApplicationDbContext context;

        public AttendancesController(IAttendanceService attendanceService, ApplicationDbContext context)
        {
            this.attendanceService = attendanceService;
            this.context = context;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Attendance.Show)]
        [HttpGet("permissions")]
        public async Task<IActionResult> GetAllPermissions()
        {
            try
            {
                var permissions = await attendanceService.GetPermissions(null, null, null);
                return Ok(permissions);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while retrieving permissions: " + ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Attendance.Show)]
        [HttpGet("attendances")] // Unique route for attendances
        public async Task<IActionResult> getAllAttendances(int? employeeId, DateTime? startDate, DateTime? endDate)
        {
            try
            {
                var attendances = await attendanceService.GetAttendanceRecords(employeeId, startDate, endDate);
                return Ok(attendances);
            }
            catch (Exception ex)
            {
                return BadRequest("An error occurred while retrieving attendances: " + ex.Message);
            }
        }

        //public async Task UpdateAttendance(AttendanceTable attendance)


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Attendance.Delete)]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteAttendance(int id)
        {
            try
            {
                await attendanceService.DeleteAttendance(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Attendance.Add)]
        [HttpPost("addpermissions")]
        public async Task<IActionResult> AddPermissions([FromForm] SaveEarlyTimeViewModel permissions)
        {
            try
            {
                await attendanceService.SaveEarlyTime(permissions);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Attendance.Show)]
        [HttpGet("Attendance/{id}")]

        public async Task<IActionResult> GetAttendanceById(int id)
        {

            try
            {
                var attendance = await attendanceService.GetAttendanceById(id);
                return Ok(attendance);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("add")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Attendance.Add)]
        public async Task<IActionResult> AddAttendance([FromForm] AttendanceTableDto dto)
        {
            try
            {
                var attendance = new AttendanceTable
                {
                    EmployeeId = dto.EmployeeId,
                    AttendanceTime = dto.AttendanceTime,
                    DepartureTime = dto.DepartureTime,
                    Date = dto.Date,
                    Bonus = dto.Bonus,
                    Discount = dto.Discount,
                    EarlyTime = dto.EarlyTime
                };

               
                AddAttendenceValidation validation = new AddAttendenceValidation(context);
                var res = validation.Validate(attendance);

                if (!res.IsValid)
                {

                    return BadRequest(res.Errors);

                }
                await attendanceService.CreateAttendance(attendance);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpPatch("edit/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Attendance.Edit)]
        public async Task<IActionResult> UpdateAttendance(int id, [FromForm] AttendanceTable attendance)
        {
            try
            {
                await attendanceService.UpdateAttendance(attendance);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
