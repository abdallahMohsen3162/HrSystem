using DataLayer.Data;
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

            if (departureTime.HasValue && attendanceTime >= departureTime.Value)
            {
                return new ValidationResult("Attendance time must be before departure time.");
            }

            return ValidationResult.Success;
        }
    }

    public class UniqueEmployeeAttendanceAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var dbContext = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
            var attendance = (AttendanceTable)validationContext.ObjectInstance;

            if (dbContext != null && attendance != null)
            {
                bool attendanceExists = dbContext.AttendanceTables
                    .Any(a => a.EmployeeId == attendance.EmployeeId &&
                              a.Date.Date == attendance.Date.Date &&
                              a.Id != attendance.Id);

                if (attendanceExists)
                {
                    return new ValidationResult("An attendance record already exists for this employee on this date.");
                }
            }

            return ValidationResult.Success;
        }
    }

}
