using BusinessLayer.Interfaces;
using DataLayer.Data;
using DataLayer.ViewModels;
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
        private readonly IDepartmentsService _departmentsService;
        public ReportController(IReportService reportService, IDepartmentsService departmentsService, ApplicationDbContext context)
        {
            _reportService = reportService;
            _context = context;
            _departmentsService = departmentsService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var employees = _context.Employee.ToList();
            ViewData["Employees"] = new SelectList(employees, "Id", "EmployeeName");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> MultiReport(int month = -1, int year = -1, int departmentId = -1)
        {
            if (month == -1 || year == -1)
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
            }

            var monthlyReports = await _reportService.GenerateMultiReportAsync(month, year,departmentId);

            ViewBag.month = month;
            ViewBag.year = year;
            ViewBag.departmentName = departmentId == -1 ? "All" : _context.Departments.Find(departmentId).DepartmentName;
            ViewBag.departments = _context.Departments.ToList();
            return View(monthlyReports);
        }

        [HttpGet]
        public async Task<IActionResult> EmployeeMonthlyReport(int month, int year, int employeeId, int departmentId)
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

        public async Task<IActionResult> DepartmentReports(int month, int year, int departmentId = -1)
        {
            Dictionary<string, List<EmployeeMonthlyReportViewModel>> mp = _reportService.GenerateDepartmentReport(month, year, departmentId);

            ViewBag.Month = month;
            ViewBag.Year = year;
            ViewBag.Departments = _departmentsService.GetDepartments(); 
            ViewBag.DepartmentId = departmentId;

            return View(mp);
        }


    }
}
