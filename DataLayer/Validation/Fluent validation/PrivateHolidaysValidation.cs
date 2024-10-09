using DataLayer.Data;
using DataLayer.Entities;
using FluentValidation;
using System.Linq;

namespace DataLayer.Validation.Fluent_validation
{
    public class PrivateHolidaysValidation : AbstractValidator<PrivateHoliday>
    {
        private readonly ApplicationDbContext _context;

        public PrivateHolidaysValidation(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x)
                .Must(NoDuplicatePrivateHoliday)
                .WithMessage("This employee already has a private holiday on the selected date.");
        }

        private bool NoDuplicatePrivateHoliday(PrivateHoliday privateHoliday)
        {
            return !_context.PrivateHolidays
                .Any(h => h.EmployeeId == privateHoliday.EmployeeId
                          && h.HolidayDate.Date == privateHoliday.HolidayDate.Date) && 
                ! _context.AttendanceTables.Any(h => h.Date.Date == privateHoliday.HolidayDate.Date && h.EmployeeId == privateHoliday.EmployeeId);
        }
    }
}
