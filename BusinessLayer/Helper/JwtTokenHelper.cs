using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Model.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLayer.Helper
{
    public  class JwtTokenHelper
    {

        private readonly IConfiguration configuration;

        public JwtTokenHelper(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string GenerateToken(UserModel user)
        {
            var claims = new[]
            {
                new Claim("UserId" , user.UserId.ToString()),
                new Claim(ClaimTypes.Email, user.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:Key"]));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(

                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: credentials

                );

            return new JwtSecurityTokenHandler().WriteToken(token);
              
            

        }
        public string GenerateResetToken(UserModel user)
        {
            var claims = new[]
            {
        new Claim("UserId", user.UserId.ToString()),
        new Claim(ClaimTypes.Email, user.Email)
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    configuration["JwtSettings:Key"]));

            var credentials =
                new SigningCredentials(
                    key,
                    SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: configuration["JwtSettings:Issuer"],
                audience: configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler()
                .WriteToken(token);
        }
    }
}
