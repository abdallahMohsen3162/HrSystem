using DataLayer.ViewModels;
using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;


namespace BusinessLayer.Interfaces
{
    public interface IAccountsService
    {
        Task<List<UserViewModel>> GetAllUsersAsync();
        Task<CreateUserViewModel> GetCreateUserViewModelAsync();
        Task<IdentityResult> CreateUserAsync(CreateUserViewModel model);
        Task<EditUserViewModel> GetEditUserViewModelAsync(string id);
        Task<IdentityResult> UpdateUserAsync(EditUserViewModel model);
        Task<DeleteUserViewModel> GetDeleteUserViewModelAsync(string id);
        Task<IdentityResult> DeleteUserAsync(string id);
        Task<IdentityResult> UpdateUserAsync(ApplicationUser user, string newPassword);
        Task<ApplicationUser> FindUserByName(string name);
        Task<SignInResult> Signin(LoginViewModel model);
        Task Signout();

        Task<ApplicationUser> getUserData(ClaimsPrincipal User);
        Task<IdentityResult> ResetPasswordAsync(ResetUserPasswordViewModel model);
    }
}
