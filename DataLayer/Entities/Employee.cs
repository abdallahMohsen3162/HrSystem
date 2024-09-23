using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataLayer.Entities
{
    public class Employee
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string EmployeeName { get; set; }

        public string ?image { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        [StringLength(50)]
        public string Nationality { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(20)]
        public string NationalId { get; set; }

        [Required]
        public DateTime JoinDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }

        [Required]
        public TimeSpan AttendanceTime { get; set; }

        [Required]
        public TimeSpan DepartureTime { get; set; }

        public GeneralSettings ?GeneralSettings { get; set; }

        [ForeignKey("Department")]
        public int? DepartmentId { get; set; }

        public Department ?Department { get; set; }
    }
}
