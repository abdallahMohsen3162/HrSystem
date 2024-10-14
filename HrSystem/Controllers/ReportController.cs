using BusinessLayer.Interfaces;
using DataLayer.Data;
using DataLayer.ViewModels;
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Rotativa.AspNetCore;
using System;
using System.Threading.Tasks;
using System.IO;
using DataLayer.Settings;
using BusinessLayer.Services;
using HrSystem.Services;
using SelectPdf;
using PdfDocument = SelectPdf.PdfDocument;
using BusinessLayer.FileManagement;
using Microsoft.AspNetCore.Authorization;
using DataLayer.Entities;


namespace HrSystem.Controllers
{
    public class ReportController : Controller
    {
        private readonly IReportService _reportService;
        private readonly ApplicationDbContext _context;
        private readonly IDepartmentsService _departmentsService;
        private readonly IConverter _converter;
        private readonly IEmailSender _emailSender;
        
        public ReportController(IEmailSender emailSender,/* IConverter converter,*/ IReportService reportService, IDepartmentsService departmentsService, ApplicationDbContext context)
        {
            _reportService = reportService;
            _context = context;
            _departmentsService = departmentsService;
            //_converter = converter;
            _emailSender = emailSender;
            
        }
        [Authorize(Policy = AuthConstants.Salary.Show)]
        [HttpGet]
        public IActionResult Index()
        {
            var employees = _context.Employee.ToList();
            ViewData["Employees"] = new SelectList(employees, "Id", "EmployeeName");
            return View();
        }


        [Authorize(Policy = AuthConstants.Salary.Show)]
        [HttpGet]
        public async Task<IActionResult> MultiReport(int month = -1, int year = -1, int departmentId = -1)
        {
            if (month == -1 || year == -1)
            {
                month = DateTime.Now.Month;
                year = DateTime.Now.Year;
            }

            var monthlyReports = await _reportService.GenerateMultiReportAsync(month, year, departmentId);

            ViewBag.month = month;
            ViewBag.year = year;
            ViewBag.departmentName = departmentId == -1 ? "All" : _context.Departments.Find(departmentId).DepartmentName;
            ViewBag.departments = _context.Departments.ToList();
            return View(monthlyReports);
        }
        [Authorize(Policy = AuthConstants.Salary.Show)]
        [HttpGet]
        public async Task<IActionResult> EmployeeMonthlyReport(int month, int year, int employeeId, int departmentId)
        {
            var report = await _reportService.GenerateEmployeeMonthlyReportAsync(month, year, employeeId);

            if (report == null)
            {
                return NotFound();
            }

            ViewBag.month = month;
            ViewBag.year = year;
            ViewBag.name = report.EmployeeName;
            ViewBag.employeeId = employeeId;
            return View(report);
        }
        [Authorize(Policy = AuthConstants.Salary.Show)]
        public async Task<IActionResult> DepartmentReports(int month, int year, int departmentId = -1)
        {
            Dictionary<string, List<EmployeeMonthlyReportViewModel>> mp = _reportService.GenerateDepartmentReport(month, year, departmentId);

            ViewBag.Month = month;
            ViewBag.Year = year;
            ViewBag.Departments = _departmentsService.GetDepartments();
            ViewBag.DepartmentId = departmentId;

            return View(mp);
        }

        [Authorize(Policy = AuthConstants.Salary.Show)]
        public async Task<IActionResult> Invoice(int id, int year, int month)
        {

            if (year == 0 || month == 0)

            {
                year = DateTime.Now.Year;
                month = DateTime.Now.Month;
            }
            EmployeeMonthlyReportViewModel report = await _reportService.GenerateEmployeeMonthlyReportAsync(month, year, id);

            return View(report);
        }



        //public async Task<FileContentResult> DownloadInvoicePdf(int id, int year, int month, bool sendEmail)
        //{
        //    if (year == 0 || month == 0)
        //    {
        //        year = DateTime.Now.Year;
        //        month = DateTime.Now.Month;
        //    }

        //    EmployeeMonthlyReportViewModel report = await _reportService.GenerateEmployeeMonthlyReportAsync(month, year, id);

        //    PdfDocument document = new PdfDocument();
        //    document.Info.Title = $"Employee Report - {report.EmployeeName}";

        //    PdfPage page = document.AddPage();
        //    XGraphics gfx = XGraphics.FromPdfPage(page);
        //    XFont fontTitle = new XFont("Verdana", 20);
        //    XFont fontContent = new XFont("Verdana", 12);

        //    gfx.DrawString("Employee Monthly Report", fontTitle, XBrushes.Black, new XRect(0, 0, page.Width, page.Height * 0.1), XStringFormats.TopCenter);

        //    gfx.DrawString($"Employee ID: {report.EmployeeId}", fontContent, XBrushes.Black, new XRect(40, 100, page.Width, page.Height), XStringFormats.TopLeft);
        //    gfx.DrawString($"Employee Name: {report.EmployeeName}", fontContent, XBrushes.Black, new XRect(40, 130, page.Width, page.Height), XStringFormats.TopLeft);
        //    gfx.DrawString($"Basic Salary: {report.BasicSalary:C}", fontContent, XBrushes.Black, new XRect(40, 160, page.Width, page.Height), XStringFormats.TopLeft);
        //    gfx.DrawString($"Attended Days: {report.AttendedDays}", fontContent, XBrushes.Black, new XRect(40, 190, page.Width, page.Height), XStringFormats.TopLeft);
        //    gfx.DrawString($"Absent Days: {report.AbsentDays}", fontContent, XBrushes.Black, new XRect(40, 220, page.Width, page.Height), XStringFormats.TopLeft);
        //    gfx.DrawString($"Bonuses (Hours): {report.BonusesByHours:C}", fontContent, XBrushes.Black, new XRect(40, 250, page.Width, page.Height), XStringFormats.TopLeft);
        //    gfx.DrawString($"Deductions (Hours): {report.DeductionsByHours:C}", fontContent, XBrushes.Black, new XRect(40, 280, page.Width, page.Height), XStringFormats.TopLeft);
        //    gfx.DrawString($"Bonuses (Money): {report.BonusesByMoney:C}", fontContent, XBrushes.Black, new XRect(40, 310, page.Width, page.Height), XStringFormats.TopLeft);
        //    gfx.DrawString($"Deductions (Money): {report.DeductionsByMoney:C}", fontContent, XBrushes.Black, new XRect(40, 340, page.Width, page.Height), XStringFormats.TopLeft);
        //    gfx.DrawString($"Net Salary: {report.NetSalary:C}", fontContent, XBrushes.Black, new XRect(40, 370, page.Width, page.Height), XStringFormats.TopLeft);
        //    gfx.DrawString($"Hourly Rate: {report.HourPrice:C}", fontContent, XBrushes.Black, new XRect(40, 400, page.Width, page.Height), XStringFormats.TopLeft);

        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        document.Save(stream, false);
        //        var pdfBytes = stream.ToArray();

        //        if (sendEmail)
        //        {
        //            string emailSubject = $"Monthly Report for {report.EmployeeName} - {month}/{year}";
        //            string emailBody = $"Please find the attached monthly report for {report.EmployeeName}.";
        //            List<IFormFile> files = new List<IFormFile>();

        //            var pdfFormFile = new FormFile(new MemoryStream(pdfBytes), 0, pdfBytes.Length, "pdf", $"{report.EmployeeName}_Report_{month}_{year}.pdf");
        //            files.Add(pdfFormFile);

        //            await _employeeService.SendEmailWithAttachmentsAsync("abdallah3162@gmail.com", emailSubject, emailBody, files);
        //        }

        //        // Return the PDF file for download
        //        return File(pdfBytes, "application/pdf", $"{report.EmployeeName}_Report_{month}_{year}.pdf");
        //    }
        //}





        [Authorize(Policy = AuthConstants.Salary.Show)]
        public async Task<FileContentResult> DownloadInvoicePdf(string html)
        {
            html = html.Replace("StrTag", "<").Replace("EndTag", ">");
            HtmlToPdf oHtmlToPdf = new HtmlToPdf();
            PdfDocument oPdfDocument = oHtmlToPdf.ConvertHtmlString(html);
            
            byte[] pdf = oPdfDocument.Save();
            oPdfDocument.Close();
            
            return File(pdf, "application/pdf", "Invoice.pdf");
        }


        [Authorize(Policy = AuthConstants.Salary.Show)]
        public async Task<IActionResult> PdfPage()
        {
            return View();
        }

        [Authorize(Policy = AuthConstants.Salary.Show)]
        [HttpPost]
        public async Task<IActionResult> UploadPdf(IFormFile file, string email)
        {
            if (email == null || string.IsNullOrEmpty(email))
            {
                return BadRequest(new { message = "Email is required" });
            }

            if (file != null && file.Length > 0)
            {
                _emailSender.SendEmailWithAttachmentsAsync(email, "subject", "body", new List<IFormFile> { file });
                return Ok(new { message = "PDF uploaded successfully!" });
            }

            return BadRequest(new { message = "No file uploaded." });
        }


    }
}
