using BusinessLayer.Interfaces;
using DataLayer.Data;
using DataLayer.Entities;
using DataLayer.Validation.Fluent_validation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrSystem.Controllers.Api
{




    [ApiController]
    [Route("api/[controller]")]
    public class HolidaysController : ControllerBase
    {

        private readonly IHolidayService _holidaysService;
        private readonly ApplicationDbContext context;
        public HolidaysController(IHolidayService holidaysService, ApplicationDbContext context)
        {
            _holidaysService = holidaysService;

            this.context = context;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Attendance.Show)]
        [HttpGet("allHolidays")]
        public async Task<IActionResult> getAllHolidays()
        {
            try
            {

                List<Holiday> holidays = await _holidaysService.GetAllHolidaysAsync();

                return Ok(holidays);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Attendance.Add)]
        public async Task<IActionResult> creatHoliday([FromForm] Holiday holiday)
        {

            try
            {
                HolidaysValidation validate = new HolidaysValidation(context);

                var res = validate.Validate(holiday);

                if (!res.IsValid)
                {

                    return BadRequest(res.Errors);

                }


                await _holidaysService.AddHolidayAsync(holiday);

                return Ok(holiday);

            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }

        [HttpPost("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Attendance.Edit)]
        public async Task<IActionResult> UpdateHoliday([FromForm] Holiday holiday)
        {

            try
            {

                await _holidaysService.UpdateHolidayAsync(holiday);
                HolidaysValidation validate = new HolidaysValidation(context);

                var res = validate.Validate(holiday);

                if (!res.IsValid)
                {

                    return BadRequest(res.Errors);

                }


                return Ok();

            }

            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }


        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Attendance.Delete)]
        public async Task<IActionResult> DeleteHoliday(int id)
        {

            try
            {

                await _holidaysService.DeleteHolidayAsync(id);

                return Ok();

            }

            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }





    }
}
