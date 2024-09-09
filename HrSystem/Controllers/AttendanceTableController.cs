using BusinessLayer.Interfaces;
using BusinessLogic.Services;
using DataLayer.Entities;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HrSystem.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly IAttendanceService _attendanceService;

        public AttendanceController(IAttendanceService attendanceService)
        {
            _attendanceService = attendanceService;
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
            ViewBag.Employees = new SelectList(await _attendanceService.GetEmployeesAsync(), "Id", "EmployeeName");
            return View("Permissions", model);
        }
    }
}
