using DataLayer.Validation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.dto.Users
{
    public class EditUserDto
    {
        public string Id { get; set; }
        [UniqueUsernameAttribute]
        public string UserName { get; set; }
        public string FullName { get; set; }
        [UniqueEmailAttribute]
        [EmailAddress]

        public string Email { get; set; }
        public string SelectedRole { get; set; }
        
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
