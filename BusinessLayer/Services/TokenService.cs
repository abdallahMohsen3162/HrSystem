//using System;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using Microsoft.Extensions.Configuration;
//using Microsoft.IdentityModel.Tokens;

//namespace BusinessLayer.Services
//{
//    public class TokenService : ITokenService
//    {
//        private readonly IConfiguration _configuration;

//        public TokenService(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        public string GenerateToken(string username)
//        {
//            var claims = new[]
//            {
//                new Claim(JwtRegisteredClaimNames.Sub, username),
//                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
//            };

//            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtConfig:Key"]));
//            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//            var token = new JwtSecurityToken(
//                issuer: _configuration["JwtConfig:Issuer"],
//                audience: _configuration["JwtConfig:Audience"],
//                claims: claims,
//                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["JwtConfig:TokenValidityInMins"])),
//                signingCredentials: creds
//            );

//            return new JwtSecurityTokenHandler().WriteToken(token);
//        }
//    }
//}
