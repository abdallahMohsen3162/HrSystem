using DataLayer.Data;
using DataLayer.ViewModels;
using FluentValidation;

public class CreateEmployeeViewModelValidator : AbstractValidator<CreateEmployeeViewModel>
{
    private readonly ApplicationDbContext _context;
    public CreateEmployeeViewModelValidator( ApplicationDbContext context)
    {
        this._context = context;

        RuleFor(x => x.EmployeeName)
            .NotEmpty().WithMessage("Employee name is required.")
            .Length(1, 100).WithMessage("Employee name must be between 1 and 100 characters.")
            .Must(x => !_context.Employee.Any(e => e.EmployeeName == x)).WithMessage("Employee name already exists.");


        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required.")
            .Length(1, 200).WithMessage("Address must be between 1 and 200 characters.");

        RuleFor(x => x.PhoneNumber)
            .Matches(@"^\d{11}$").WithMessage("Invalid phone number (must be 11 digits).");

        RuleFor(x => x.Gender)
            .NotEmpty().WithMessage("Gender is required.")
            .Length(1, 10).WithMessage("Gender must be between 1 and 10 characters.");

        RuleFor(x => x.Nationality)
            .NotEmpty().WithMessage("Nationality is required.")
            .Length(1, 50).WithMessage("Nationality must be between 1 and 50 characters.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("Date of birth is required.")
            .LessThan(DateTime.Today).WithMessage("Date of birth must be in the past.");

        RuleFor(x => x.NationalId)
            .NotEmpty().WithMessage("National ID is required.")
            .Matches(@"^\d+$").WithMessage("Invalid National ID (must be numbers only).")
            .Must(x => !_context.Employee.Any(e => e.NationalId == x)).WithMessage("National ID already exists.");

        RuleFor(x => x.JoinDate)
            .NotEmpty().WithMessage("Join date is required.");

        RuleFor(x => x.Salary)
            .GreaterThan(0).WithMessage("Salary must be a positive number.");

        RuleFor(x => x.AttendanceTime)
            .LessThan(x => x.DepartureTime).WithMessage("Attendance time must be before departure time.");

        RuleFor(x => x.ProfileImage)
            .NotNull().WithMessage("Profile image is required.");
    }
}
