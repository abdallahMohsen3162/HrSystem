﻿using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HrSystem.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly IDepartmentsService _departmentService;




        public DepartmentsController(IDepartmentsService departmentService)
        {
            _departmentService = departmentService;
        }
        public IActionResult Index()
        {
            var departments = _departmentService.GetDepartments();

            //ViewData["Departments"] = new SelectList(departments, "Id", "DepartmentName");
            return View(departments);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Create(Department department)
        {
            if (ModelState.IsValid)
            {
                await _departmentService.AddDepartment(department);
                return RedirectToAction(nameof(Index));
            }

            return View(department);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var department = await _departmentService.GetDepartmentById(id);
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Department department)
        {
            if (ModelState.IsValid)
            {
                await _departmentService.UpdateDepartment(department);

                return RedirectToAction(nameof(Index));
            }
            return View(department);
        }

        public IActionResult DeleteConfirmed(int id)
        {
            id = (int)id;
            Console.WriteLine("DeleteConfirmed: " + id);
            Console.WriteLine("DeleteConfirmed: " + id);
            Console.WriteLine("DeleteConfirmed: " + id);
            Console.WriteLine("DeleteConfirmed: " + id);
            Console.WriteLine("DeleteConfirmed: " + id);
            _departmentService.DeleteDepartment(id);
            return RedirectToAction(nameof(Index));
        }

        [AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> ValidateDepartmentName(string departmentName, int id)
        {
            var result = await _departmentService.GetDepartmentByName(departmentName);
            if (result == null || (result != null && result.Id == id))
            {
                return Json(true);
            }
            return Json("Department already exists");
        }



    }
}