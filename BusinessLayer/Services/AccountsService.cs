using BusinessLayer.Interfaces;
using DataLayer.ViewModels;
using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace BusinessLayer.Services
{
    public class AccountsService : IAccountsService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountsService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
        }

        public async Task<List<UserViewModel>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            return users.Select(user => new UserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                Fullname = user.Fullname,
                Email = user.Email
            }).ToList();
        }

        public async Task<CreateUserViewModel> GetCreateUserViewModelAsync()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return new CreateUserViewModel
            {
                AvailableRoles = roles
            };
        }

        public async Task<IdentityResult> CreateUserAsync(CreateUserViewModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Username,
                Fullname = model.FullName,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded) return result;

            return await _userManager.AddToRoleAsync(user, model.Role);
        }

        public async Task<EditUserViewModel> GetEditUserViewModelAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return null;
            }

            var roles = await _roleManager.Roles.ToListAsync();
            var selectedRole = (await _userManager.GetRolesAsync(user)).FirstOrDefault();

            return new EditUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.Fullname,
                Email = user.Email,
                AvailableRoles = roles,
                SelectedRole = selectedRole
            };
        }

        public async Task<IdentityResult> UpdateUserAsync(EditUserViewModel model)
        {
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }

            if (!string.IsNullOrEmpty(model.Password))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, model.Password);
                if (!result.Succeeded)
                {
                    return result;
                }
            }

            user.UserName = model.UserName;
            user.Fullname = model.FullName;
            user.Email = model.Email;

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded) return updateResult;

            var roles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, roles);
            return await _userManager.AddToRoleAsync(user, model.SelectedRole);
        }

        public async Task<DeleteUserViewModel> GetDeleteUserViewModelAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user == null ? null : new DeleteUserViewModel
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.Fullname,
                Email = user.Email
            };
        }

        public async Task<IdentityResult> DeleteUserAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            return user == null
                ? IdentityResult.Failed(new IdentityError { Description = "User not found" })
                : await _userManager.DeleteAsync(user);
        }

        public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user, string newPassword)
        {
            if (!string.IsNullOrEmpty(newPassword))
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
                if (!result.Succeeded)
                {
                    return result;
                }
            }

            return await _userManager.UpdateAsync(user);
        }

        public async Task<ApplicationUser> FindUserByName(string name)
        {
            return await _userManager.FindByNameAsync(name);
        }

        public async Task<SignInResult> Signin(LoginViewModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.UserName);

                if (user == null && model.UserName.Contains("@", StringComparison.OrdinalIgnoreCase))
                {
                    user = await _userManager.FindByEmailAsync(model.UserName);
                }

                if (user != null)
                {
                    return await _signInManager.PasswordSignInAsync(user.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                }

                return SignInResult.Failed;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error during sign-in: {ex.Message}");
                return SignInResult.Failed;
            }
        }
        public async Task Signout()
        {
            await _signInManager.SignOutAsync();
        }
    }
}