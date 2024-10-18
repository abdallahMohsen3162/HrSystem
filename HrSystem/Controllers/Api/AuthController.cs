using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataLayer.Entities;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HrSystem.Controllers.Api
{
    public class AuthController : ControllerBase
    {
        private readonly IAccountsService _accountsService;
        private readonly IConfiguration _configuration;
        private readonly IRolesService _rolesService;
        private readonly UserManager<ApplicationUser> _userManager;
        public AuthController(UserManager<ApplicationUser> userManager,IAccountsService accountsService, IConfiguration configuration, IRolesService rolesService)
        {
            _accountsService = accountsService;
            _configuration = configuration;
            _rolesService = rolesService;
            _userManager = userManager;
        }

        [HttpGet("get-user-profile")]
        public async Task<ActionResult<object>> GetUserProfile()
        {

            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader))
            {
                return BadRequest("Authorization header not found");
            }
            var token = authHeader.Substring("Bearer ".Length).Trim();

            var username = User.Identity.Name;

            var user = await _accountsService.FindUserByName(username);

            return Ok(new
            {
                username=user.UserName,
                email=user.Email

            });
        }


        [HttpPost("login")]
        public async Task<ActionResult<object>> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            //var result = await _accountsService.Signin(model);
            var user = await _accountsService.FindUserByName(model.UserName);
            if (await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var role = await _accountsService.getUserRole(user.Id);
                var token = GenerateToken(model.UserName, role);

                return new
                {
                    message = "Login successful",
                    token,
                    role
                };

            }
            return Unauthorized();
        }

        private async Task<string> GenerateToken(string username, string rolename)
        {

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, username),
        new Claim(ClaimTypes.Role, rolename) 
    };


            var roleClaims = await _rolesService.GetClaimsByRoleNameAsync(rolename);

            foreach (var roleClaim in roleClaims)
            {
                claims.Add(new Claim(roleClaim.Type, roleClaim.Type));
            }


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["JwtConfig:Issuer"],
                audience: _configuration["JwtConfig:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(1), 
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
