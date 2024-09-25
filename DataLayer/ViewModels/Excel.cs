using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class AttendanceSheet
    {
        public int ?Id { get; set; }
        public string ?EmployeeName { get; set; }
        public int ?EmployeeId { get; set; }
        public TimeSpan ?In { get; set; }
        public TimeSpan ?Out { get; set; }
        public string valid { get; set; }
        public DateTime History { get; set; } 

    }
}
