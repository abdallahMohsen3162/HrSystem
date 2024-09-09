using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.Validation;
using Microsoft.AspNetCore.Mvc;

namespace DataLayer.Entities
{
    public class PrivateHoliday
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Remote(action: "ValidateAttendanceRecord", controller: "PrivateHolidays", AdditionalFields = nameof(HolidayDate))]
        public int EmployeeId { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        
        public Employee? Employee { get; set; }

        [Required]
        [Remote(action: "ValidateHolidayDate", controller: "PrivateHolidays", AdditionalFields = "EmployeeId")]
        public DateTime HolidayDate { get; set; }

    }    
}
