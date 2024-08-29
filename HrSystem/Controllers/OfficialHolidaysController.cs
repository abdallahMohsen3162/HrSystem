using DataLayer.Data;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using DataLayer.Models; 





namespace HrSystem.Controllers 
{

    public class DateTimeMonster
    {
        public static string GetDayNameByDate(int day, int month, int year)
        {
            if (month == 1 || month == 2)
            {
                month += 12;
                year -= 1;
            }
            int K = year % 100; 
            int J = year / 100;
            int h = (day + (13 * (month + 1)) / 5 + K + (K / 4) + (J / 4) - 2 * J) % 7;
            string[] dayNames = { "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
            return dayNames[h];
        }
    }
    

    public class HolidaysController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HolidaysController(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<IActionResult> Index()
        {
            var model = await _context.Holidays.ToListAsync();
            List<string> days = new List<string>();
            foreach (var holiday in model)
            {
                string str = DateTimeMonster.GetDayNameByDate(holiday.Date.Day, holiday.Date.Month, holiday.Date.Year);
                days.Add(str);
            }
            ViewBag.days = days;
            return View(model);
        }


        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Date")] Holiday holiday)
        {
            if (ModelState.IsValid)
            {
                _context.Add(holiday);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(holiday);
        }


        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holiday = await _context.Holidays.FindAsync(id);
            if (holiday == null)
            {
                return NotFound();
            }
            return View(holiday);
        }


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
                    _context.Update(holiday);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HolidayExists(holiday.Id))
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


        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var holiday = await _context.Holidays
                .FirstOrDefaultAsync(m => m.Id == id);
            
            _context.Holidays.Remove(holiday);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }



        private bool HolidayExists(int id)
        {
            return _context.Holidays.Any(e => e.Id == id);
        }
    }
}
