using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DataLayer.Validation;
using Microsoft.AspNetCore.Identity;

namespace DataLayer.ViewModels
{
    public class CreateUserViewModel
    {
        [Required]
        [Display(Name = "Username")]
        [UniqueUsernameAttribute]
        public string Username { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        [UniqueEmailAttribute]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string Role { get; set; }

        public List<IdentityRole> AvailableRoles { get; set; } = new List<IdentityRole>();
    }
}
