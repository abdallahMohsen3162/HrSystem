using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Validation;

namespace DataLayer.Entities
{
    public class AttendanceTable
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Employee")]
        public int EmployeeId { get; set; }

        public Employee Employee { get; set; }

        [Required]
        [TimeSpanValidation(ErrorMessage = "Attendance time must be before departure time.")]
        public DateTime AttendanceTime { get; set; }

        public DateTime? DepartureTime { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
