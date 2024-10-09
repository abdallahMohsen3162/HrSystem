using DataLayer.Data;
using DataLayer.Entities;
using FluentValidation;
using System;
using System.Linq;

namespace DataLayer.Validation.Fluent_validation
{
    public class AddDepartmentValidator : AbstractValidator<Department>
    {
        private readonly ApplicationDbContext _context;

        public AddDepartmentValidator(ApplicationDbContext context)
        {
            _context = context;

            RuleFor(x => x.DepartmentName)
                .NotEmpty().WithMessage("Department name is required.")
                .Length(1, 100).WithMessage("Department name must be between 1 and 100 characters.")
                .Must((department, name) => !_context.Departments
                    .Any(d => d.DepartmentName == name && d.Id != department.Id))
                .WithMessage("Department name already exists.");
        }
    }
}
