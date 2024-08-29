using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace DataLayer.Validation
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Retrieve UserManager from DI
            var userManager = validationContext.GetService(typeof(UserManager<ApplicationUser>)) as UserManager<ApplicationUser>;

            // Check if value is a string (the email)
            if (value is not string email || string.IsNullOrWhiteSpace(email))
            {
                return new ValidationResult("Email is required.");
            }


            // Retrieve the user ID from the model
            var userId = validationContext.ObjectInstance.GetType().GetProperty("Id")?.GetValue(validationContext.ObjectInstance) as string;

            if (userManager == null)
            {
                return new ValidationResult("Unable to access UserManager.");
            }

            // Find user by email
            var existingUser = userManager.FindByEmailAsync(email).Result;
            
            // Check if a user with the same email but different ID exists
            if (existingUser != null && existingUser.Id != userId)
            {
                return new ValidationResult("The email is already taken by another user.");
            }

            return ValidationResult.Success;
        }
    }

    public class UniqueUsernameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Retrieve UserManager from DI
            var userManager = validationContext.GetService(typeof(UserManager<ApplicationUser>)) as UserManager<ApplicationUser>;

            // Check if value is a string (the username)
            if (value is not string username || string.IsNullOrWhiteSpace(username))
            {
                return new ValidationResult("Username is required.");
            }

            // Retrieve the user ID from the model
            var userId = validationContext.ObjectInstance.GetType().GetProperty("Id")?.GetValue(validationContext.ObjectInstance) as string;

            if (userManager == null)
            {
                return new ValidationResult("Unable to access UserManager.");
            }

            // Find user by username
            var existingUser = userManager.FindByNameAsync(username).Result;

            // Check if a user with the same username but different ID exists
            if (existingUser != null && existingUser.Id != userId)
            {
                return new ValidationResult("The username is already taken by another user.");
            }

            return ValidationResult.Success;
        }
    }
}
