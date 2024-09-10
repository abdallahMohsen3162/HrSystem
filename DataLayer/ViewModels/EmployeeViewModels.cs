using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Configuration;
using DataLayer.Entities;
using DataLayer.Validation;


namespace DataLayer.ViewModels
{

    public class CreateEmployeeViewModel
    {
        [Required]
        [StringLength(100)]
        public string EmployeeName { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Invalid phone number (0-xx-xxxx-xxxx)")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        [StringLength(50)]
        
        public string Nationality { get; set; }

        [Required]
        [MinAge(18, ErrorMessage = "Employee must be at least 18 years old.")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid Nationality (Numbers only)")]
        public string NationalId { get; set; }

        [Required]
        public DateTime JoinDate { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }

        [Required]
        [TimeSpanValidation(ErrorMessage = "Attendance time must be before departure time.")]
        public TimeSpan AttendanceTime { get; set; }

        [Required]
        public TimeSpan DepartureTime { get; set; }

        [Required]
        
        public IFormFile ProfileImage { get; set; }


        public int ?DepartmentId { get; set; }



    }



    public class EditeEmployeeViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string EmployeeName { get; set; }

        [Required]
        [StringLength(200)]
        public string Address { get; set; }

        [Required]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Error : Ex(01xxxxxxxx) Max count is 11 number")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(10)]
        public string Gender { get; set; }

        [Required]
        [StringLength(50)]
        //only numbers
        
        public string Nationality { get; set; }

        [Required]
        [MinAge(18, ErrorMessage = "Employee must be at least 18 years old.")]
        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(20)]
        [RegularExpression(@"^\d+$", ErrorMessage = "Invalid Nationality (Numbers only)")]
        public string NationalId { get; set; }

        [Required]
        public DateTime JoinDate { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Salary must be a positive number.")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salary { get; set; }

        [Required]
        [TimeSpanValidationAttributeEdit(ErrorMessage = "Attendance time must be before departure time.")]
        public TimeSpan AttendanceTime { get; set; }

        [Required]
        public TimeSpan DepartureTime { get; set; }
        
        public IFormFile? ProfileImage { get; set; }
        [Required]
        public int? DepartmentId { get; set; }
    }

}
