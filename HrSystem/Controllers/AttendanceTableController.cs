using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataLayer.Entities;
using DataLayer.Data; 
using System.Threading.Tasks;
using System.Linq;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using static DataLayer.Entities.AuthConstants;


namespace HrSystem.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index(int? employeeId, DateTime? startDate, DateTime? endDate)
        {
            var attendanceQuery = _context.AttendanceTables.Include(a => a.Employee).AsQueryable();

            if (employeeId.HasValue)
            {
                attendanceQuery = attendanceQuery.Where(a => a.EmployeeId == employeeId.Value);
            }

            if (startDate.HasValue)
            {
                attendanceQuery = attendanceQuery.Where(a => a.Date >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                attendanceQuery = attendanceQuery.Where(a => a.Date <= endDate.Value);
            }

            var attendanceList = await attendanceQuery.ToListAsync();

            ViewData["Employees"] = new SelectList(await _context.Employee.ToListAsync(), "Id", "EmployeeName");
            return View(attendanceList);
        }



        public IActionResult Create()
        {
            ViewBag.Employees = new SelectList(_context.Employee, "Id", "EmployeeName");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AttendanceTable attendance)
        {
            if (ModelState.IsValid)
            {
                var employee = await _context.Employee.FindAsync(attendance.EmployeeId);
                if (employee != null)
                {

                    attendance.Discount = 0;
                    attendance.Bonus = 0;
                    attendance.Discount += Math.Max((decimal)(employee.DepartureTime - attendance.DepartureTime)?.TotalHours, 0);
                    attendance.Discount += Math.Max((decimal)(attendance.AttendanceTime - employee.AttendanceTime).TotalHours, 0);
                    attendance.Bonus += Math.Max((decimal)(employee.DepartureTime - attendance.DepartureTime)?.TotalHours, 0);


                    _context.Add(attendance);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }


                ModelState.AddModelError("", "Employee not found");
            }

            ViewBag.Employees = new SelectList(_context.Employee, "Id", "EmployeeName", attendance.EmployeeId);
            return View(attendance);
        }



        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.AttendanceTables.FindAsync(id);
            if (attendance == null)
            {
                return NotFound();
            }

            ViewBag.Employees = new SelectList(_context.Employee, "Id", "EmployeeName", attendance.EmployeeId);
            return View(attendance);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AttendanceTable attendance)
        {
            if (id != attendance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var employee = await _context.Employee.FindAsync(attendance.EmployeeId);
                    attendance.Discount = 0;
                    attendance.Bonus = 0;
                    attendance.Discount += Math.Max((decimal)(employee.DepartureTime - attendance.DepartureTime)?.TotalHours, 0);
                    attendance.Discount += Math.Max((decimal)(attendance.AttendanceTime - employee.AttendanceTime).TotalHours, 0);
                    attendance.Bonus += Math.Max((decimal)(attendance.DepartureTime - employee.DepartureTime)?.TotalHours, 0);
                    _context.Update(attendance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.AttendanceTables.Any(e => e.Id == attendance.Id))
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
            foreach (var error in ViewData.ModelState.Values.SelectMany(v => v.Errors))
            {
                Console.WriteLine(error.ErrorMessage);
            }
            ViewBag.Employees = new SelectList(_context.Employee, "Id", "EmployeeName", attendance.EmployeeId);
            return View(attendance);
        }



        public async Task<IActionResult> Delete(int id)
        {
            var attendance = await _context.AttendanceTables.FindAsync(id);
            _context.AttendanceTables.Remove(attendance);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
