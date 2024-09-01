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
        public TimeSpan AttendanceTime { get; set; } 

        public TimeSpan? DepartureTime { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
