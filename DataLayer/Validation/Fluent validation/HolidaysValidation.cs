using FluentValidation;
using DataLayer.Entities;
using DataLayer.Data;

namespace DataLayer.Validation.Fluent_validation
{
    public class HolidaysValidation : AbstractValidator<Holiday>
    {
        private readonly ApplicationDbContext _context;

        public HolidaysValidation(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(h => h.Name)
                .NotEmpty().WithMessage("Holiday name is required.")
                .Must((holiday, name) => !_context.Holidays.Any(h => h.Name == name && h.Id != holiday.Id))
                .WithMessage("Holiday name must be unique.");

            RuleFor(h => h.Date)
                .NotEmpty().WithMessage("Holiday date is required.");
        }
    }
}
