using BusinessLayer.Interfaces;
using BusinessLogic.Services;
using DataLayer.Data;
using DataLayer.Entities;
using DataLayer.Migrations;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HrSystem.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IAttendanceService _attendanceService;
        private readonly ApplicationDbContext _context;
        public AttendanceController(IAttendanceService attendanceService,
            ApplicationDbContext context)
        {
            _attendanceService = attendanceService;
            _context = context;
        }

        public async Task<IActionResult> Index(int? employeeId, DateTime? startDate, DateTime? endDate)
        {
            var attendanceList = await _attendanceService.GetAttendanceRecords(employeeId, startDate, endDate);
            ViewData["Employees"] = new SelectList(await _attendanceService.GetEmployeesAsync(), "Id", "EmployeeName");
            return View(attendanceList);
        }

        public IActionResult Create()
        {
            ViewBag.Employees = new SelectList(_attendanceService.GetEmployeesAsync().Result, "Id", "EmployeeName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AttendanceTable attendance)
        {
            if (ModelState.IsValid)
            {
                await _attendanceService.CreateAttendance(attendance);
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Employees = new SelectList(await _attendanceService.GetEmployeesAsync(), "Id", "EmployeeName", attendance.EmployeeId);
            return View(attendance);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var attendance = await _attendanceService.GetAttendanceById(id.Value);
            if (attendance == null) return NotFound();

            ViewBag.Employees = new SelectList(await _attendanceService.GetEmployeesAsync(), "Id", "EmployeeName", attendance.EmployeeId);
            return View(attendance);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AttendanceTable attendance)
        {
            if (id != attendance.Id) return NotFound();

            if (ModelState.IsValid)
            {
                await _attendanceService.UpdateAttendance(attendance);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Employees = new SelectList(await _attendanceService.GetEmployeesAsync(), "Id", "EmployeeName", attendance.EmployeeId);
            return View(attendance);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _attendanceService.DeleteAttendance(id);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Permissions(int? employeeId, DateTime? startDate, DateTime? endDate)
        {
            ViewData["Employees"] = new SelectList(await _attendanceService.GetEmployeesAsync(), "Id", "EmployeeName");
            var attendanceList = await _attendanceService.GetPermissions(employeeId, startDate, endDate);
            return View(attendanceList);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEarlyTime(SaveEarlyTimeViewModel model)
        {
            if (ModelState.IsValid)
            {
                await _attendanceService.SaveEarlyTime(model);
                return RedirectToAction(nameof(Permissions));
            }
            ViewData["Employees"] = new SelectList(await _attendanceService.GetEmployeesAsync(), "Id", "EmployeeName");
            return View("Permissions", model);
        }

        [HttpGet]
        public async Task<IActionResult> CreatePermession()
        {
            ViewData["Employees"] = new SelectList(await _attendanceService.GetEmployeesAsync(), "Id", "EmployeeName");
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ValidateDate(DateTime date, int employeeId)
        {
            var attendanceRecord = await _context.AttendanceTables
                .FirstOrDefaultAsync(a => a.EmployeeId == employeeId && a.Date == date);

            Console.WriteLine(date);
            Console.WriteLine(date);
            Console.WriteLine(date);
            Console.WriteLine(employeeId);
            Console.WriteLine(employeeId);
            Console.WriteLine(employeeId);
            Console.WriteLine(employeeId);

            if (attendanceRecord == null)
            {
                return Json("Attendance record does not exist for the specified date");
            }

            var permissionTaken = await _context.AttendanceTables
                .AnyAsync(p => p.EmployeeId == employeeId && p.Date == date && p.EarlyTime != null && p.EarlyTime > 0);

            if (permissionTaken)
            {
                return Json("The employee has already taken permission on this date");
            }

            return Json(true); 
        }

        [HttpPost]
        public async Task<IActionResult> EditPermissions(AttendanceTable model)
        {
            if (ModelState.IsValid)
            {
                await _attendanceService.UpdateAttendance(model);
                return RedirectToAction(nameof(Permissions));
            }

            ViewData["Employees"] = new SelectList(await _attendanceService.GetEmployeesAsync(), "Id", "EmployeeName", model.EmployeeId);
            return View("Permissions", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditPermissions(int id)
        {
            var res = await _attendanceService.GetAttendanceById(id);
            
            return View(res);
        }

    }
}
