using BusinessLayer.Interfaces;
using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DataLayer.ViewModels;

using System.Web.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace BusinessLayer.Services
{
    public class RolesService : IRolesService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RolesService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }


      
        public List<IdentityRole> GetRoles()
        {
            var roles = _roleManager.Roles.ToList();
            return roles;
        }

        public async Task<IdentityResult> CreateRoleAsync(string roleName)
        {
            if (string.IsNullOrWhiteSpace(roleName))
            {
                throw new ArgumentException("Role name must be provided.", nameof(roleName));
            }

            var roleExists = await _roleManager.RoleExistsAsync(roleName);
            if (!roleExists)
            {
                var role = new IdentityRole(roleName);
                return await _roleManager.CreateAsync(role);
            }

            return IdentityResult.Failed(new IdentityError { Description = "Role already exists." });
        }

        public async Task<IdentityRole> GetRoleByNameAsync(string roleName)
        {
            return await _roleManager.FindByNameAsync(roleName);
        }

        public async Task<IdentityResult> DeleteRoleAsync(string roleId)
        {

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                return await _roleManager.DeleteAsync(role);
            }
                

            return IdentityResult.Failed(new IdentityError { Description = "Role not found." });
        }

        public async Task<IdentityResult> AddClaimToRoleAsync(string roleId, string claimType, string claimValue)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role != null)
            {
                var claim = new System.Security.Claims.Claim(claimType, claimValue);
                return await _roleManager.AddClaimAsync(role, claim);
            }

            return IdentityResult.Failed(new IdentityError { Description = "Role not found." });
        }

        public async Task<IdentityResult> AssignClaimToRoleAsync(string roleId, string claimType, string claimValue)
        {

            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                throw new ArgumentException("Role not found", nameof(roleId));
            }
            var claim = new Claim(claimType, claimValue);
            var result = await _roleManager.AddClaimAsync(role, claim);

            return result;
        }
        public async Task<List<Claim>> GetClaimsByRoleNameAsync(string roleName)
        {
            var role = await _roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                return new List<Claim>();
            }

            var claims = await _roleManager.GetClaimsAsync(role);
            return claims.ToList();
        }

        public async Task<IdentityRole> GetRoleByIdAsync(string roleId)
        {
            return await _roleManager.FindByIdAsync(roleId);
        }

        public async Task<IdentityResult> UpdateRoleAsync(IdentityRole role)
        {
            return await _roleManager.UpdateAsync(role);
        }

        public async Task<IdentityResult> RemoveClaimFromRoleAsync(string roleId, string claimType, string claimValue)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null)
            {
                throw new ArgumentException("Role not found", nameof(roleId));
            }
            var claim = new Claim(claimType, claimValue);
            var result = await _roleManager.RemoveClaimAsync(role, claim);

            return result;
        }

    }
}
