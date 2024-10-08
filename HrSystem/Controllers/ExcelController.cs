﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
//using OfficeOpenXml;
using System.Threading.Tasks;
using BusinessLayer.FileManagement;
using OfficeOpenXml;
using DataLayer.Data;
using Microsoft.VisualBasic;
using DataLayer.Entities;
using BusinessLayer.Interfaces;
using BusinessLogic.Services;
using DataLayer.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace HrSystem.Controllers
{
    public class ExcelController : Controller
    {   
        private ApplicationDbContext _context;
        private readonly IAttendanceService _attendanceService;
        public ExcelController(ApplicationDbContext _context, IAttendanceService _attendanceService)
        {
            this._context = _context;
            this._attendanceService = _attendanceService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }


        /*
       [HttpPost]
       public async Task<IActionResult> UploadExcel(IFormFile file)
       {
           if (file == null || file.Length == 0)
           {
               Console.WriteLine("No file uploaded or file is empty.");
               return RedirectToAction("NowAllowd", "Errors");
           }

           string name = file.FileName;
           if (!name.EndsWith(".xlsx"))
           {
               Console.WriteLine("Invalid file format. Only .xlsx files are allowed.");
               return RedirectToAction("NowAllowd", "Errors");
           }


           FileManager fileManager = new FileManager("wwwroot"); 
           string relativeFilePath = await fileManager.SaveFileAsync(file, "excel");

           string filePath = Path.Combine("wwwroot", relativeFilePath);
           Console.WriteLine($"File saved at: {filePath}");


           if (!System.IO.File.Exists(filePath))
           {
               Console.WriteLine("File does not exist after upload.");
               return RedirectToAction("NowAllowd", "Errors");
           }
           ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

           FileInfo fileInfo = new FileInfo(filePath);
           using (var package = new ExcelPackage(fileInfo))
           {
               ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

               int rowCount = worksheet.Dimension?.Rows ?? 0;
               int colCount = worksheet.Dimension?.Columns ?? 0;

               if (rowCount == 0 || colCount == 0)
               {
                   Console.WriteLine("empty");
                   return RedirectToAction("NowAllowd", "Errors");
               }
               Dictionary<int, string> NameOfColumn = new Dictionary<int, string>();
               for (int col = 1; col <= colCount; col++)
               {
                   var cellValue = worksheet.Cells[2, col].Text;
                   NameOfColumn.Add(col, cellValue);
               }

               var historyCell = worksheet.Cells[1, 3]; // Cell object
               var historyText = historyCell.Text; // Extract the text from the cell

               Console.WriteLine($"History: {historyText}");

               DateTime historyDate; // Variable to store the parsed DateTime
               bool isHistoryValid = DateTime.TryParseExact(historyText, "d-M-yyyy", null, System.Globalization.DateTimeStyles.None, out historyDate);



               for (int row = 3; row <= rowCount; row++)
               {
                   var empId = worksheet.Cells[row, 2].Text;
                   var attendanceTimeStr = worksheet.Cells[row, 3].Text;
                   var depTimeStr = worksheet.Cells[row, 4].Text;

                   if (!string.IsNullOrEmpty(empId) && !string.IsNullOrEmpty(attendanceTimeStr) && !string.IsNullOrEmpty(depTimeStr))
                   {
                       Console.WriteLine($"Employee ID: {empId}, Attendance Time: {attendanceTimeStr}, Departure Time: {depTimeStr}");

                       DateTime attendanceTime;
                       DateTime depTime;

                       bool isAttendanceTimeValid = DateTime.TryParse(attendanceTimeStr, out attendanceTime);
                       bool isDepTimeValid = DateTime.TryParse(depTimeStr, out depTime);
                        Console.WriteLine($"Attendance Time Valid: {isAttendanceTimeValid}, Departure Time Valid: {isDepTimeValid}");
                       if (isAttendanceTimeValid && isDepTimeValid)
                       {
                           var employee = _context.Employee.Find(int.Parse(empId));

                           if (employee != null)
                           {
                               TimeSpan startTime = attendanceTime.TimeOfDay;
                               TimeSpan endTime = depTime.TimeOfDay;
                               _attendanceService.CreateAttendance(
                                   new AttendanceTable
                                   {
                                       EmployeeId = employee.Id,
                                       AttendanceTime = startTime,
                                       DepartureTime = endTime,
                                       Date = historyDate, 
                                       Bonus = 0, 
                                       Discount = 0,
                                       EarlyTime = 0
                                   }
                                );

                           }
                           else
                           {
                               Console.WriteLine($"Employee with ID {empId} not found.");
                           }
                       }
                       else
                       {
                           Console.WriteLine($"Invalid attendance or departure time for Employee ID: {empId}");
                       }
                   }
                   else
                   {
                       Console.WriteLine("Empty Employee ID, Attendance Time, or Departure Time");
                   }
               }

           }

           return RedirectToAction("Index");
       }*/



        public async Task<IActionResult> UploadExcel(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                Console.WriteLine("No file uploaded or file is empty.");
                return RedirectToAction("NowAllowd", "Errors");
            }

            string name = file.FileName;
            if (!name.EndsWith(".xlsx"))
            {
                Console.WriteLine("Invalid file format. Only .xlsx files are allowed.");
                return RedirectToAction("NowAllowd", "Errors");
            }

            List<AttendanceSheet> lst = new List<AttendanceSheet>();

            FileManager fileManager = new FileManager("wwwroot");
            string relativeFilePath = await fileManager.SaveFileAsync(file, "excel");

            string filePath = Path.Combine("wwwroot", relativeFilePath);
            Console.WriteLine($"File saved at: {filePath}");

            if (!System.IO.File.Exists(filePath))
            {
                Console.WriteLine("File does not exist after upload.");
                return RedirectToAction("NowAllowd", "Errors");
            }

            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            FileInfo fileInfo = new FileInfo(filePath);
            DateTime historyDate;
            using (ExcelPackage package = new ExcelPackage(fileInfo))
            {
              
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];


                string dateString = worksheet.Cells["C1"].Text;
                DateTime date;
                if (!DateTime.TryParse(dateString, out date))
                {
                    Console.WriteLine("Date format is incorrect or missing.");
                    return RedirectToAction("NotAllowed", "Errors");
                }
                historyDate = date;
                int startRow = 3;
                int rowCount = worksheet.Dimension.Rows;
                var employees = _context.Employee.ToList();
                for (int row = startRow; row <= rowCount; row++)
                {
                    try
                    {
        
                        var no = worksheet.Cells[row, 1].Text; 
                        var employeeId = worksheet.Cells[row, 2].Text; 
                        var attendanceTimeStr = worksheet.Cells[row, 3].Text; 
                        var departureTimeStr = worksheet.Cells[row, 4].Text; 
                        var emp = employees.FirstOrDefault(x => x.Id == int.Parse(employeeId));

                        TimeSpan attendanceTime = DateTime.Parse(attendanceTimeStr).TimeOfDay;
                        TimeSpan departureTime = DateTime.Parse(departureTimeStr).TimeOfDay;

                        string val = "valid";
                        if (emp == null)
                        {
                            val = "no data";
                        }
                        Console.WriteLine(historyDate.Day);
                        Console.WriteLine(historyDate.Month);
                        Console.WriteLine(historyDate.Year);
                        Console.WriteLine("############");
                        if (_context.AttendanceTables.Any(x => x.EmployeeId == int.Parse(employeeId) && x.Date.Year == date.Year && x.Date.Month == date.Month && x.Date.Day == date.Day) == true)
                        {
                            val = "Exists before";
                        }

                        lst.Add(new AttendanceSheet
                        {
                            Id = int.Parse(no),
                            EmployeeId = int.Parse(employeeId),
                            EmployeeName = emp?.EmployeeName ,
                            In = attendanceTime,
                            Out = departureTime,
                            History = date,
                            valid = val
                           
                        });



                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing row {row - 1}: {ex.Message}");
                    }
                }
            }


            ViewBag.historyDate = historyDate;
            return View(lst);
        }


        [HttpPost]
        public async Task<IActionResult> SaveAttendances(List<AttendanceSheet> AttendanceSheets, List<int> selectedRecords)
        {
            

            int affectedRows = 0;

            foreach (var emp in AttendanceSheets)
            {
                if (selectedRecords.Contains((int)emp.Id))
                {
                    _attendanceService.CreateAttendance(
                        new AttendanceTable
                        {
                            EmployeeId = (int)emp.EmployeeId,
                            AttendanceTime = (TimeSpan)emp.In,
                            DepartureTime = emp.Out,
                            Date = emp.History,
                        }
                    );
                    affectedRows++; 
                }
            }


            TempData["AffectedRows"] = affectedRows;

            return RedirectToAction(nameof(Feedback)); 
        }


        public IActionResult Feedback()
        {
            ViewBag.AffectedRows = TempData["AffectedRows"] ?? 0;
            return View();
        }




    }
}