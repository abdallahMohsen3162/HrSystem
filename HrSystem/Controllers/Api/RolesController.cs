using AutoMapper;
using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataLayer.Data;
using DataLayer.dto.Roles;
using DataLayer.dto.Users;
using DataLayer.Entities;
using DataLayer.Validation;
using DataLayer.Validation.Fluent_validation;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;


namespace HrSystem.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private IAccountsService _accountsService;
        private IMapper _mapper;
        private ApplicationDbContext _context;
        IRolesService _rolesService;
        public RolesController(IAccountsService accountsService, IMapper mapper, ApplicationDbContext context,IRolesService rolesService)
        {
            _accountsService = accountsService;
            _mapper = mapper;
            _context = context;
            _rolesService = rolesService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Permissions.Show)]
        [HttpGet("allRoles")]
        public ActionResult<List<Rolesdto>> getAllRoles()
        {
            try{
                List<Rolesdto> roles = _mapper.Map<List<Rolesdto>>(_rolesService.GetRoles());

                return roles;
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Permissions.Edit)]
        [HttpPatch("editRole/{id}")]
        public async Task<IActionResult> UpdateRole(string id, [FromForm] List<string> claims)
        {
            try
            {
                var role = await _rolesService.GetRoleByIdAsync(id);
                if (role == null)
                {
                    return BadRequest("Role not found");
                }
                List<Claim> existingClaims = await _rolesService.GetClaimsByRoleNameAsync(role.Id);

                foreach (var claim in Claims.AllClaims)
                {
                   await _rolesService.RemoveClaimFromRoleAsync(role.Id, claim.Type, claim.Value);
                }
                foreach (var claim in claims)
                {
                    await _rolesService.AssignClaimToRoleAsync(id, claim, claim);
                }

                return Ok("Role updated successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = AuthConstants.Permissions.Delete)]
        [HttpDelete("deleteRole/{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            try
            {
                var role = await _rolesService.GetRoleByIdAsync(id);
                if (role == null)
                {
                    return BadRequest("Role not found");
                }
                await _rolesService.DeleteRoleAsync(role.Id);

                return Ok("Role deleted successfully");
            }
                catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }





    }
}
