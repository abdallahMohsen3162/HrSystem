
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BusinessLayer.Interfaces;
using DataLayer.Data;
using Microsoft.AspNetCore.Authorization;

namespace HrSystem.Controllers
{
    public class HolidaysController : Controller
    {
        private readonly BusinessLayer.Interfaces.IHolidayService _holidayService;
        private readonly ApplicationDbContext _context;
        public HolidaysController(BusinessLayer.Interfaces.IHolidayService holidayService, ApplicationDbContext context)
        {
            _holidayService = holidayService;
            _context = context;
        }
        [Authorize(Policy = AuthConstants.Attendance.Show)]
        public async Task<IActionResult> Index()
        {
            var model = await _holidayService.GetAllHolidaysAsync();
            ViewBag.days = _holidayService.GetHolidayDaysOfWeek(model);
            return View(model);
        }
        [Authorize(Policy = AuthConstants.Attendance.Add)]
        public IActionResult Create()
        {
            return View();
        }


        [Authorize(Policy = AuthConstants.Attendance.Add)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Date")] Holiday holiday)
        {
            if (ModelState.IsValid)
            {
                await _holidayService.AddHolidayAsync(holiday);
                return RedirectToAction(nameof(Index));
            }
            return View(holiday);
        }


        [Authorize(Policy = AuthConstants.Attendance.Edit)]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("NotFound", "Errors");
            }

            var holiday = await _holidayService.GetHolidayByIdAsync(id.Value);
            if (holiday == null)
            {
                return RedirectToAction("NotFound", "Errors");
            }
            return View(holiday);
        }


        [Authorize(Policy = AuthConstants.Attendance.Edit)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Date")] Holiday holiday)
        {
            if (id != holiday.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _holidayService.UpdateHolidayAsync(holiday);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_holidayService.HolidayExists(holiday.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(holiday);
        }



        [Authorize(Policy = AuthConstants.Attendance.Delete)]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            await _holidayService.DeleteHolidayAsync(id.Value);
            return RedirectToAction(nameof(Index));
        }
    }
}
