using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataLayer.ViewModels;
using DataLayer.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace HrSystem.Controllers
{
    public class RolesController : Controller
    {
        private readonly IRolesService _rolesService;

        public RolesController(IRolesService rolesService)
        {
            _rolesService = rolesService;
        }

        public IActionResult Index()
        {
            List<IdentityRole> roles = _rolesService.GetRoles();
            List<RoleClaimViewModel> model = new List<RoleClaimViewModel>();
            foreach (var role in roles)
            {
                List<Claim> claims = _rolesService.GetClaimsByRoleNameAsync(role.Name).Result as List<Claim>;
                model.Add(new RoleClaimViewModel
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    Claims = claims
                });
            }
            
            return View(model);
        }
        [Authorize(Policy = AuthConstants.Permissions.Show)]
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [Authorize(Policy = AuthConstants.Permissions.Add)]
        [HttpPost]
        public async Task<IActionResult> CreateRole(CreateRoleClaimsViewModel model)
        {
            if (model.Claims == null)
            {
                model.Claims = new List<string>();
            }
            if (ModelState.IsValid)
            {
                // Check if the role name already exists
                var existingRole = await _rolesService.GetRoleByNameAsync(model.RoleName);
                if (existingRole != null)
                {
                    // Add error for RoleName field
                    ModelState.AddModelError("RoleName", "Name must be unique");
                }
                else
                {
                    // Proceed with role creation
                    var result = await _rolesService.CreateRoleAsync(model.RoleName);
                    if (result.Succeeded)
                    {
                        var role = await _rolesService.GetRoleByNameAsync(model.RoleName);
                        foreach (var claim in model.Claims)
                        {
                            var claimResult = await _rolesService.AssignClaimToRoleAsync(role.Id, claim, claim);
                        }
                        return RedirectToAction("Index");
                    }
                }
            }


           
            ModelState.AddModelError("RoleName", "Name must be unique");
            
            Console.WriteLine("Error creating role");

            return View(nameof(Create), model);
        }



        [Authorize(Policy = AuthConstants.Permissions.Delete)]
        [HttpPost]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            Console.WriteLine(roleId);
            var result = await _rolesService.DeleteRoleAsync(roleId);
            if (result.Succeeded)
            {
                // Optionally add success message or redirect
                return RedirectToAction("Index");
            }
            else
            {
                
                return RedirectToAction("Index");
            }
        }

        [Authorize(Policy = AuthConstants.Permissions.Add)]
        [HttpPost]
        public async Task<IActionResult> AddClaimToRole(string roleId, string claimType, string claimValue)
        {
            var result = await _rolesService.AddClaimToRoleAsync(roleId, claimType, claimValue);
            return RedirectToAction("Index");
        }

        [Authorize(Policy = AuthConstants.Permissions.Edit)]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Permissions.Edit)]
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            var role = await _rolesService.GetRoleByIdAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            var AllClaims = Claims.AllClaims;

            var claims = await _rolesService.GetClaimsByRoleNameAsync(role.Name);
            foreach (var claim in claims)
            {
                Console.WriteLine(claim.Type + " " + claim.Value);
            }
            var model = new EditRoleClaimsViewModel
            {
                RoleId = role.Id,
                RoleName = role.Name,
                Claims = claims.Select(c => c.Value).ToList()
            };
            

            return View(model);
        }

        [HttpPost]
        [Authorize(Policy = AuthConstants.Permissions.Edit)]
        public async Task<IActionResult> Edit(EditRoleClaimsViewModel model)
        {
            if (ModelState.IsValid)
            {
                var role = await _rolesService.GetRoleByIdAsync(model.RoleId);
                if (role == null)
                {
                    return NotFound();
                }

                role.Name = model.RoleName;
                var updateResult = await _rolesService.UpdateRoleAsync(role);
                if (updateResult.Succeeded)
                {
                    var existingClaims = await _rolesService.GetClaimsByRoleNameAsync(role.Name);
                    foreach (var claim in existingClaims)
                    {
                        await _rolesService.RemoveClaimFromRoleAsync(role.Id, claim.Type, claim.Value);
                    }

                    foreach (var claim in model.Claims)
                    {
                        await _rolesService.AddClaimToRoleAsync(role.Id, claim, claim);
                    }

                    return RedirectToAction("Index");
                }

                foreach (var error in updateResult.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

    }
}
