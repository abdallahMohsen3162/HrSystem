using DataLayer.Validation;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace DataLayer.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Fullname { get; set; }
        public string Email { get; set; }
    }

    public class EditUserViewModel
    {
        public string Id { get; set; }
        [UniqueUsernameAttribute]
        public string UserName { get; set; }
        public string FullName { get; set; }
        [UniqueEmailAttribute]
        [EmailAddress]
        
        public string Email { get; set; }
        public string SelectedRole { get; set; }
        public IList<IdentityRole>? AvailableRoles { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

    }

    public class DeleteUserViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
    }
}
