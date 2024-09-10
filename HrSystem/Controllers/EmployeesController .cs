using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using BusinessLayer.Services;
using DataLayer.ViewModels;
using BusinessLayer.Interfaces;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HrSystem.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly IEmployeeService _employeeService;
        private readonly IDepartmentsService _departmentsService;
        
        public EmployeesController(IEmployeeService employeeService, IDepartmentsService departmentsService)
        {
            _employeeService = employeeService;
            _departmentsService = departmentsService;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
            var model = await _employeeService.GetAllEmployeesAsync();
            
            return View(model);
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {

            List<ListItem> nationalities = Nationalities.GetNationalities();
            ViewBag.nationalities = nationalities;
            ViewBag.departments = _departmentsService.GetDepartments();
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateEmployeeViewModel employee)
        {
            if (ModelState.IsValid)
            {
                await _employeeService.CreateEmployeeAsync(employee);
                return RedirectToAction(nameof(Index));
            }
            List<ListItem> nationalities = Nationalities.GetNationalities();
            ViewBag.nationalities = nationalities;
            ViewBag.departments = _departmentsService.GetDepartments();
            return View(employee);
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            var model = new EditeEmployeeViewModel
            {
                Id = employee.Id,
                EmployeeName = employee.EmployeeName,
                Address = employee.Address,
                PhoneNumber = employee.PhoneNumber,
                Gender = employee.Gender,
                Nationality = employee.Nationality,
                DateOfBirth = employee.DateOfBirth,
                NationalId = employee.NationalId,
                JoinDate = employee.JoinDate,
                Salary = employee.Salary,
                AttendanceTime = employee.AttendanceTime,
                DepartureTime = employee.DepartureTime,
                DepartmentId = employee.DepartmentId,
                
            };
            List<ListItem> nationalities = Nationalities.GetNationalities();
            ViewBag.nationalities = nationalities;
            ViewBag.imageUrl = employee.image;
            var departments = _departmentsService.GetDepartments();
            ViewBag.departments = departments;
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditeEmployeeViewModel employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _employeeService.UpdateEmployeeAsync(id, employee);
                return RedirectToAction(nameof(Index));
            }
            List<ListItem> nationalities = Nationalities.GetNationalities();
            ViewBag.nationalities = nationalities;
            ViewBag.departments = _departmentsService.GetDepartments();
            return View(employee);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _employeeService.GetEmployeeByIdAsync(id.Value);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employee/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _employeeService.DeleteEmployeeAsync(id);
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _employeeService.EmployeeExists(id);
        }
    }
}
