using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using DataLayer.Entities;
using DataLayer.ViewModels;
using Microsoft.AspNetCore.Authorization;
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
        public AuthController(IAccountsService accountsService, IConfiguration configuration, IRolesService rolesService)
        {
            _accountsService = accountsService;
            _configuration = configuration;
            _rolesService = rolesService;
        }

        [Authorize]
        [HttpGet("get-user-profile")]
        public async Task<IActionResult> GetUserProfile()
        {

            var authHeader = Request.Headers["Authorization"].ToString();
            if (string.IsNullOrEmpty(authHeader))
            {
                return BadRequest("Authorization header not found");
            }
            var token = authHeader.Substring("Bearer ".Length).Trim();

            var username = User.Identity.Name;

            var user = await _accountsService.FindUserByName(username);
            Console.WriteLine(user.UserName);
            Console.WriteLine(user.Email);
            Console.WriteLine(user);

            return Ok(new
            {
                username=user.UserName,
            });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var result = await _accountsService.Signin(model);
            var user = await _accountsService.FindUserByName(model.UserName);
            if (result == null || user == null)
            {
                return Unauthorized(new { message = "Invalid credentials" });
            }
            var role = await _accountsService.getUserRole(user.Id);
            var token = GenerateToken(model.UserName, role);

            return Ok(new
            {
                message = "Login successful",
                token
            });
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _accountsService.Signout();
            return Ok(new { message = "Logout successful" });
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
