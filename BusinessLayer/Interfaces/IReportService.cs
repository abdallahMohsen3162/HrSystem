using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface IReportService
    {
        Task<List<EmployeeMonthlyReportViewModel>> GenerateMultiReportAsync(int month, int year, int departmentId);
        Dictionary<string, List<EmployeeMonthlyReportViewModel>> GenerateDepartmentReport(int month, int year, int departmentId = -1);
        Task<EmployeeMonthlyReportViewModel> GenerateEmployeeMonthlyReportAsync(int month, int year, int employeeId);
    }
}
