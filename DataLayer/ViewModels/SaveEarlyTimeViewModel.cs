using DataLayer.Validation;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.ViewModels
{
    public class SaveEarlyTimeViewModel
    {
        [Required]
        public int EmployeeId { get; set; }

        [Required]
        [Range(0, 8, ErrorMessage = "Early Time should be between 0 and 8 hours.")]
        
        public decimal Hours { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Remote(action: "ValidateDate", controller: "Attendance", AdditionalFields=nameof(EmployeeId))]
        public DateTime Date { get; set; }
    }
}
