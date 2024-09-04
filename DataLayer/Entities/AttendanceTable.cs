using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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

        public Employee? Employee { get; set; }

        [Required]
        [TimeRangeValidation]
        
        public TimeSpan AttendanceTime { get; set; }
        [Required]
        public TimeSpan? DepartureTime { get; set; }

        [Required]
        [UniqueEmployeeAttendance]
        public DateTime Date { get; set; }

        public decimal ?Bonus { get; set; }
        public decimal ?Discount { get; set; }
        [Range(0,8)]
        public decimal ?EarlyTime { get; set; }

    }
}
