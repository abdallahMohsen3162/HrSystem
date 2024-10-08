﻿using BusinessLayer.Interfaces;
using BusinessLogic.Services;
using DataLayer.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Threading.Tasks;

namespace HrSystem.Controllers
{
    public class PrivateHolidaysController : Controller
    {
        private readonly IPrivateHolidayService _privateHolidayService;

        public PrivateHolidaysController(IPrivateHolidayService privateHolidayService)
        {
            _privateHolidayService = privateHolidayService;
        }
        [Authorize(Policy = AuthConstants.Attendance.Show)]
        public async Task<IActionResult> Index()
        {
            var holidays = await _privateHolidayService.GetAllPrivateHolidays();
            return View(holidays);
        }
        [Authorize(Policy = AuthConstants.Attendance.Add)]
        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_privateHolidayService.GetAllEmployees().Result, "Id", "EmployeeName");
            return View();
        }


        [Authorize(Policy = AuthConstants.Attendance.Add)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeId,HolidayDate")] PrivateHoliday privateHoliday)
        {
            if (ModelState.IsValid)
            {
                await _privateHolidayService.CreatePrivateHoliday(privateHoliday);
                return RedirectToAction(nameof(Index));
            }

            ViewData["EmployeeId"] = new SelectList(await _privateHolidayService.GetAllEmployees(), "Id", "EmployeeName", privateHoliday.EmployeeId);
            return View(privateHoliday);
        }


        [Authorize(Policy = AuthConstants.Attendance.Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            var privateHoliday = await _privateHolidayService.GetPrivateHolidayById(id.Value);
            ViewData["EmployeeId"] = new SelectList(await _privateHolidayService.GetAllEmployees(), "Id", "EmployeeName", privateHoliday.EmployeeId);
            return View(privateHoliday);
        }


        [Authorize(Policy = AuthConstants.Attendance.Edit)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,HolidayDate")] PrivateHoliday privateHoliday)
        {
            if (ModelState.IsValid)
            {
                await _privateHolidayService.UpdatePrivateHoliday(privateHoliday);
                return RedirectToAction(nameof(Index));
            }

            ViewData["EmployeeId"] = new SelectList(await _privateHolidayService.GetAllEmployees(), "Id", "EmployeeName", privateHoliday.EmployeeId);
            return View(privateHoliday);
        }


        [Authorize(Policy = AuthConstants.Attendance.Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
            await _privateHolidayService.DeletePrivateHoliday(id.Value);
            return RedirectToAction(nameof(Index));
        }


        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> ValidateHolidayDate(DateTime holidayDate, int employeeId, int? id)
        {
            var result = await _privateHolidayService.ValidateHolidayDate(holidayDate, employeeId, id);
            var recordsResult = await _privateHolidayService.ValidateAttendanceRecord(employeeId, holidayDate);
            if (recordsResult)
            {
                return result ? Json(true) : Json("Attendance already exists for the specified employee and date.");
            }

            return result ? Json(true) : Json("This holiday already exists or falls on a weekly holiday.");
        }
    }
}
