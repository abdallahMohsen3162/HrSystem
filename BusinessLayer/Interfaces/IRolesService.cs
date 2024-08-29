using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Web.Mvc;
using DataLayer.Entities;
using System.Security.Claims;
using System.Data;
using DataLayer.ViewModels;


namespace BusinessLayer.Interfaces
{
    public interface IRolesService
    {
        List<IdentityRole> GetRoles();
        Task<IdentityResult> CreateRoleAsync(string roleName);

        Task<IdentityResult> DeleteRoleAsync(string roleId);
        Task<IdentityRole> GetRoleByNameAsync(string roleName);
        Task<IdentityResult> AddClaimToRoleAsync(string roleId, string claimType, string claimValue);
        Task<IdentityResult> AssignClaimToRoleAsync(string roleId, string claimType, string claimValue);
        //get claims of role
        Task<List<Claim>> GetClaimsByRoleNameAsync(string roleName);
        Task<IdentityRole> GetRoleByIdAsync(string roleId);
        Task<IdentityResult> UpdateRoleAsync(IdentityRole role);
        Task<IdentityResult> RemoveClaimFromRoleAsync(string roleId,string claimType,string claimValue);
    }
}
