using BusinessLayer.Interfaces;
using DataLayer.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;


namespace HrSystem.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly ApplicationDbContext _context;

        public ReportController(IReportService reportService, ApplicationDbContext context)
        {
            _reportService = reportService;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var employees = _context.Employee.ToList();
            ViewData["Employees"] = new SelectList(employees, "Id", "EmployeeName");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MultiReport(int month = -1, int year = -1)
        {
            if (month == -1 || year == -1)
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
            }

            var monthlyReports = await _reportService.GenerateMultiReportAsync(month, year);

            ViewBag.month = month;
            ViewBag.year = year;
            return View(monthlyReports);
        }

        [HttpGet]
        public async Task<IActionResult> EmployeeMonthlyReport(int month, int year, int employeeId)
        {
            var report = await _reportService.GenerateEmployeeMonthlyReportAsync(month, year, employeeId);

            if (report == null)
            {
                return NotFound();
            }

            ViewBag.month = month;
            ViewBag.year = year;
            ViewBag.name = report.EmployeeName;
            ViewBag.employeeId = employeeId;
            return View(report);
        }
    }
}
