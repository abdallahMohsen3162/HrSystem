using BusinessLayer.Interfaces;
using DataLayer.Entities;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HrSystem.Controllers.Api
{

    public class ReportsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public ReportsController(IReportService reportService)
        {
            _reportService = reportService;
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Salary.Show)]
        [HttpGet("MultiReport/{month}/{year}/{departmentId}")]
        public async Task<IActionResult> Reports(int month = -1, int year = -1, int departmentId = -1)
        {
            try
            {
                if (month == -1 || year == -1)
                {
                    month = DateTime.Now.Month;
                    year = DateTime.Now.Year;
                }

                List<EmployeeMonthlyReportViewModel> reports = await _reportService.GenerateMultiReportAsync(month, year, departmentId);
                return Ok(reports);
            }

            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Salary.Show)]
        [HttpGet("MonthlyReport/{month}/{year}/{employeeId}")]
        public async Task<IActionResult> EmployeeMonthlyReport(int month = -1, int year = -1, int employeeId = -1)
        {
            try
            {

                if (month == -1 || year == -1)
                {
                    month = DateTime.Now.Month;
                    year = DateTime.Now.Year;
                }
                EmployeeMonthlyReportViewModel report = await _reportService.GenerateEmployeeMonthlyReportAsync(month, year, employeeId);
                return Ok(report);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //public async Task<IActionResult> DepartmentReports(int month, int year, int departmentId = -1)
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Salary.Show)]
        [HttpGet("DepartmentReports/{month}/{year}/{departmentId}")]
        public async Task<IActionResult> DepartmentReports(int month = -1, int year = -1, int departmentId = -1)
        {
            try
            {

                if (month == -1 || year == -1)
                {
                    month = DateTime.Now.Month;
                    year = DateTime.Now.Year;
                }

                Dictionary<string, List<EmployeeMonthlyReportViewModel>> mp = _reportService.GenerateDepartmentReport(month, year, departmentId);

                return Ok(mp);

            }

            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }

        }


    }
}
