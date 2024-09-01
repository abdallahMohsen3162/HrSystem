using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataLayer;
using DataLayer.Entities;
using DataLayer.Data;

namespace HrSystem.Controllers
{

    public class AttendanceTableController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceTableController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: AttendanceTable
        public async Task<IActionResult> Index()
        {
            var attendanceTables = await _context.AttendanceTables.Include(a => a.Employee).ToListAsync();
            return View(attendanceTables);
        }

        // GET: AttendanceTable/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AttendanceTable/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,EmployeeId,AttendanceTime,DepartureTime,Date")] AttendanceTable attendanceTable)
        {
            if (ModelState.IsValid)
            {
                _context.Add(attendanceTable);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(attendanceTable);
        }

        // GET: AttendanceTable/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendanceTable = await _context.AttendanceTables.FindAsync(id);
            if (attendanceTable == null)
            {
                return NotFound();
            }
            return View(attendanceTable);
        }

        // POST: AttendanceTable/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,AttendanceTime,DepartureTime,Date")] AttendanceTable attendanceTable)
        {
            if (id != attendanceTable.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendanceTable);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceTableExists(attendanceTable.Id))
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
            return View(attendanceTable);
        }

        // GET: AttendanceTable/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendanceTable = await _context.AttendanceTables
                .FirstOrDefaultAsync(m => m.Id == id);
            if (attendanceTable == null)
            {
                return NotFound();
            }

            return View(attendanceTable);
        }

        // POST: AttendanceTable/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendanceTable = await _context.AttendanceTables.FindAsync(id);
            _context.AttendanceTables.Remove(attendanceTable);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceTableExists(int id)
        {
            return _context.AttendanceTables.Any(e => e.Id == id);
        }
    }
}
