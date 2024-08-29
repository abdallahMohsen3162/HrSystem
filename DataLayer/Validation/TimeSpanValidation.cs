
using DataLayer.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.ViewModels;
namespace DataLayer.Validation
{
    public class TimeSpanValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (CreateEmployeeViewModel)validationContext.ObjectInstance;
            if (model.AttendanceTime >= model.DepartureTime)
            {
                return new ValidationResult(ErrorMessage ?? "Invalid time range.");
            }
            return ValidationResult.Success;
        }
    }

    public class TimeSpanValidationAttributeEdit : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var model = (EditeEmployeeViewModel)validationContext.ObjectInstance;
            if (model.AttendanceTime >= model.DepartureTime)
            {
                return new ValidationResult(ErrorMessage ?? "Invalid time range.");
            }
            return ValidationResult.Success;
        }
    }

    public class MinAgeAttribute : ValidationAttribute
    {
        private readonly int _minAge;

        public MinAgeAttribute(int minAge)
        {
            _minAge = minAge;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime dateOfBirth)
            {
                var today = DateTime.Today;
                var age = today.Year - dateOfBirth.Year;

                if (dateOfBirth.Date > today.AddYears(-age))
                {
                    age--;
                }

                if (age < _minAge)
                {
                    return new ValidationResult(ErrorMessage ?? $"Employee must be at least {_minAge} years old.");
                }
            }
            return ValidationResult.Success;
        }
    }


}
