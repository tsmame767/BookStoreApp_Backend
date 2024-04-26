using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.JWT
{
    public class TokenGenerator
    {
        private readonly IConfiguration _configuration;

        public TokenGenerator(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public string GenerateJwtToken(int UserId, string Email, string Role)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Secret"]);

            // Ensure the key size is at least 256 bits (32 bytes)
            if (key.Length < 32)
            {
                throw new ArgumentException("JWT secret key must be at least 256 bits (32 bytes)");
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, UserId.ToString()),
                    new Claim(ClaimTypes.Email, Email), // Include user email as a claim
                    new Claim(ClaimTypes.Role,Role),  // We can add more claims if needed

                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        //public string GenerateJwtToken(int UserId, string Email, string Role)
        //{
        //    if (string.IsNullOrEmpty(Email))
        //    {
        //        throw new ArgumentNullException(nameof(Email), "Email cannot be null or empty.");
        //    }

        //    if (string.IsNullOrEmpty(Role))
        //    {
        //        throw new ArgumentNullException(nameof(Role), "Role cannot be null or empty.");
        //    }

        //    if (string.IsNullOrEmpty(UserId.ToString()))
        //    {
        //        throw new ArgumentNullException(nameof(UserId), "Role cannot be null or empty.");
        //    }
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //    //claim is used to add identity to JWT token
        //    var claims = new[]
        //    {
        //        new Claim(ClaimTypes.Email,Email),
        //        new Claim(ClaimTypes.NameIdentifier, UserId.ToString()),
        //        new Claim(ClaimTypes.Role,Role)
        //    };

        //    var Token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
        //        _configuration["Jwt:Audience"],
        //        claims,
        //        expires:DateTime.Now.AddMinutes(120),

        //        signingCredentials:credentials);

        //    string data=new JwtSecurityTokenHandler().WriteToken(Token);
        //    return data;
        //}
    }
}
