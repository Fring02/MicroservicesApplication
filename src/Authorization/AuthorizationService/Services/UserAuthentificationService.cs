using AuthorizationService.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthorizationService.Services
{
    public class UserAuthentificationService : JwtAuthentificationService<User>
    {
        public UserAuthentificationService(IConfiguration configuration) : base(configuration)
        {

        }
        public override ClaimsIdentity CreateIdentity(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Authentication, "true"),
                new Claim(ClaimTypes.Role, user.Role)
            };
            return new ClaimsIdentity(claims);
        }


        public override string CreateToken(ClaimsIdentity identity)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentification:Token"]));
            var token = new JwtSecurityToken(
                claims: identity.Claims,
                audience: _configuration.GetSection("Authentification:Audience").Value,
                issuer: _configuration.GetSection("Authentification:Issuer").Value,
                expires: DateTime.Now.AddHours(double.Parse(_configuration.GetSection("Authentification:Lifetime").Value)),
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
                );
            var handler = new JwtSecurityTokenHandler();
            return handler.WriteToken(token);
        }
    }
}
