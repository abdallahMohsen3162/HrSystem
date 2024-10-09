using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.dto.Attendence
{
    public class AttendanceTableDto
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        public TimeSpan AttendanceTime { get; set; }

        [Required]
        public TimeSpan? DepartureTime { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public decimal? Bonus { get; set; }

        public decimal? Discount { get; set; }

        [Range(0, 8)]
        public decimal? EarlyTime { get; set; }
    }
}
