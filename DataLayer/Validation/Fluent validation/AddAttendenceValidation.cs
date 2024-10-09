using DataLayer.Data;
using DataLayer.Entities;
using FluentValidation;
using System;
using System.Linq;

namespace DataLayer.Validation.Fluent_validation
{
    public class AddAttendenceValidation : AbstractValidator<AttendanceTable>
    {
        private readonly ApplicationDbContext _context;

        public AddAttendenceValidation(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x)
                .Must(NoDuplicateAttendance)
                .WithMessage("The employee has already attended for the selected date.");

        }

        private bool NoDuplicateAttendance(AttendanceTable attendance)
        {
            // Check if there's any attendance record for the same employee on the same day
            return !_context.AttendanceTables
                .Any(a => a.EmployeeId == attendance.EmployeeId
                          && a.Date.Date == attendance.Date.Date);
        }

    }
}
