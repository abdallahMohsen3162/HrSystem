using DataLayer.Data;
using DataLayer.ViewModels;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DataLayer.Validation.Fluent_validation
{
    public class CreateUserValidation : AbstractValidator<CreateUserViewModel>
    {
        private readonly ApplicationDbContext _context;

        public CreateUserValidation(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Username is required")
                .MustAsync(BeUniqueUsername).WithMessage("Username already exists");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Invalid email format")
                .MustAsync(BeUniqueEmail).WithMessage("Email already exists");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .MinimumLength(6).WithMessage("Password must be at least 6 characters long");

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password).WithMessage("The password and confirmation password do not match.");
        }

        private async Task<bool> BeUniqueUsername(string username, CancellationToken token)
        {
            return !await _context.Users.AnyAsync(u => u.UserName == username, token);
        }

        private async Task<bool> BeUniqueEmail(string email, CancellationToken token)
        {
            return !await _context.Users.AnyAsync(u => u.Email == email, token);
        }
    }
}
