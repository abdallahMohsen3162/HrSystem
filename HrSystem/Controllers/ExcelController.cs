using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
//using OfficeOpenXml;
using System.Threading.Tasks;
using BusinessLayer.FileManagement;
using OfficeOpenXml;

namespace HrSystem.Controllers
{
    public class ExcelController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Add()
        {
            return View();
        }

        public IActionResult UploadExcel()
        {
            return View();
        }

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
                foreach(var item in NameOfColumn)
                {
                    Console.WriteLine($"Column {item.Key}: {item.Value}");
                }

                for (int row = 3; row <= rowCount; row++)
                {

                    Console.WriteLine($"Row {row}:");
                    for (int col = 1; col <= colCount; col++)
                    {
                        var cellValue = worksheet.Cells[row, col].Text;
                        Console.Write(cellValue);
                        Console.Write(", ");
                    }
                    Console.WriteLine();
                }
            }

            return RedirectToAction("Index");
        }

    }
}