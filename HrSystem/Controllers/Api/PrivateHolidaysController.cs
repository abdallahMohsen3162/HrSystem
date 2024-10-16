using BusinessLayer.Interfaces;
using DataLayer.Data;
using DataLayer.dto.Attendence;
using DataLayer.Entities;
using DataLayer.Validation.Fluent_validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrSystem.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrivateHolidaysController : ControllerBase
    {


        private readonly IPrivateHolidayService _privateHolidaysService;

        private readonly ApplicationDbContext context;




        public PrivateHolidaysController(IPrivateHolidayService privateHolidaysService, ApplicationDbContext context)
        {
            _privateHolidaysService = privateHolidaysService;

            this.context = context;

        }



        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Attendance.Show)]
        [HttpGet]

        public async Task<IActionResult> getAllPrivateHolidays()
        {
            try
            {
                List<PrivateHoliday> privateHolidays = await _privateHolidaysService.GetAllPrivateHolidays();
                return Ok(privateHolidays);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Attendance.Delete)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {

            try
            {

                await _privateHolidaysService.DeletePrivateHoliday(id);
                return Ok();

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Attendance.Add)]
        [HttpPost]
        public async Task<IActionResult> AddPrivateHoliday([FromForm] PrivateHolidayDto privateHoliday)
        {

            try
            {
                PrivateHoliday privateHoliday1 = new PrivateHoliday
                {
                    HolidayDate = privateHoliday.HolidayDate,
                    EmployeeId = privateHoliday.EmployeeId,
                };
                PrivateHolidaysValidation validator = new PrivateHolidaysValidation(context);

                var res = validator.Validate(privateHoliday1);

                if (!res.IsValid)
                {

                    return BadRequest(res.Errors);

                }


                await _privateHolidaysService.CreatePrivateHoliday(privateHoliday1);
                return Ok();

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }



    }
}
