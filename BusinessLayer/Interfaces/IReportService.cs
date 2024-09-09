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
        Task<List<EmployeeMonthlyReportViewModel>> GenerateMultiReportAsync(int month, int year);
        Task<EmployeeMonthlyReportViewModel> GenerateEmployeeMonthlyReportAsync(int month, int year, int employeeId);
    }
}
