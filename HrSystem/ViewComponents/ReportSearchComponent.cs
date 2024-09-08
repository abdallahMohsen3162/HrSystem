using DataLayer.Data; // Assuming your DbContext is in the Data namespace
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HrSystem.ViewComponents
{
    public class ReportSearchViewComponent : ViewComponent
    {
        private readonly ApplicationDbContext _context; 

        public ReportSearchViewComponent(ApplicationDbContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var employees = _context.Employee.ToList();


            ViewData["Employees"] = new SelectList(employees, "Id", "EmployeeName");

            return View();
        }
    }
}
