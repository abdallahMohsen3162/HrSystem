using DataLayer.Entities;
using System;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.Validation
{
    public class TimeRangeValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var attendance = (AttendanceTable)validationContext.ObjectInstance;
            var attendanceTime = attendance.AttendanceTime;
            var departureTime = attendance.DepartureTime;

            if (departureTime.HasValue && attendanceTime > departureTime.Value)
            {
                return new ValidationResult("Attendance time must be before departure time.");
            }

            return ValidationResult.Success;
        }
    }
}
