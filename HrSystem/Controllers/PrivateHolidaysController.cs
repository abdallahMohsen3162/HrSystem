using DataLayer.Data;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HrSystem.Controllers
{
    public class PrivateHolidaysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PrivateHolidaysController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var holidays = await _context.PrivateHolidays.Include(h => h.Employee).ToListAsync();
            return View(holidays);
        }


        public IActionResult Create()
        {
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "Id", "EmployeeName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeId,HolidayDate")] PrivateHoliday privateHoliday)
        {
            if (ModelState.IsValid)
            {

                _context.Add(privateHoliday);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["EmployeeId"] = new SelectList(_context.Employee, "Id", "EmployeeName", privateHoliday.EmployeeId);
            return View(privateHoliday);
        }


        public async Task<IActionResult> Edit(int? id)
        {

            var privateHoliday = await _context.PrivateHolidays.FindAsync(id);
            ViewData["EmployeeId"] = new SelectList(_context.Employee, "Id", "EmployeeName", privateHoliday.EmployeeId);
            return View(privateHoliday);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,HolidayDate")] PrivateHoliday privateHoliday)
        {

            if (ModelState.IsValid)
            {
                _context.Update(privateHoliday);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }

            ViewData["EmployeeId"] = new SelectList(_context.Employee, "Id", "EmployeeName", privateHoliday.EmployeeId);
            return View(privateHoliday);
        }


        public async Task<IActionResult> Delete(int? id)
        {
            var privateHoliday = await _context.PrivateHolidays.FindAsync(id);
            _context.PrivateHolidays.Remove(privateHoliday);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [AcceptVerbs("Get", "Post")]
        public IActionResult ValidateAttendanceRecord(int employeeId, DateTime holidayDate)
        {
            
            var attendanceRecord = _context.AttendanceTables
                .FirstOrDefault(a => a.EmployeeId == employeeId && a.Date == holidayDate);

            if (attendanceRecord != null)
            {
                return Json(true);
            }
            return Json("Attendance does not exist for the specified employee and date.");
        }


        private bool PrivateHolidayExists(int id)
        {
            return _context.PrivateHolidays.Any(e => e.Id == id);
        }
    }
}
