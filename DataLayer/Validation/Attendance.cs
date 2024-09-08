using DataLayer.Data;
using DataLayer.Entities;
using DataLayer.ViewModels;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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


    //public class ExistingAttendanceRecordAttribute : ValidationAttribute
    //{
    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        var model = (SaveEarlyTimeViewModel)validationContext.ObjectInstance;
    //        var _context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));

    //        var attendanceRecord = _context.AttendanceTables
    //            .FirstOrDefault(a => a.EmployeeId == model.EmployeeId && a.Date == model.Date);

    //        if (attendanceRecord != null)
    //        {
    //            return ValidationResult.Success; 
    //        }

    //        return new ValidationResult("Attendance does not exist for the specified employee and date");
    //    }
    //}


    public class PrivateHolidayExistsInHolidaysAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (PrivateHoliday)validationContext.ObjectInstance;
            var _context = (ApplicationDbContext)validationContext.GetService(typeof(ApplicationDbContext));
            var dayExistsBefore = _context.PrivateHolidays.Where(p => p.HolidayDate == model.HolidayDate && p.EmployeeId == model.EmployeeId && p.Id != model.Id).FirstOrDefault();
            var officialHolidays = _context.Holidays.Where(h => h.Date == model.HolidayDate).FirstOrDefault();
            if (dayExistsBefore != null || officialHolidays != null)
            {
                return new ValidationResult("This holiday already exists.");
            }

        
    
            int empId = model.EmployeeId;

            var dayOfWeek = model.HolidayDate.DayOfWeek;

            var employeeGeneralSettings = _context.GeneralSettings.FirstOrDefault(gs => gs.EmployeeId == empId);
            HashSet<int> st = new HashSet<int>();
            if (employeeGeneralSettings != null)
            {
  
                var weeklyHolidays = employeeGeneralSettings.WeeklyHolidayList.ToList();
                for(int i = 0; i < weeklyHolidays.Count; i++)
                {
                    int x = (int)weeklyHolidays[i];
                    st.Add(x);
                }

                if (st.Contains((int)dayOfWeek))
                {
                    return new ValidationResult("The selected date falls on a weekly holiday.");
                }
            }

            return ValidationResult.Success;
        }
    }


}
