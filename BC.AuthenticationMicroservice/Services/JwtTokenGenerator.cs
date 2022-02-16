using BC.AuthenticationMicroservice.Interfaces;
using BC.AuthenticationMicroservice.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BC.AuthenticationMicroservice.Services
{
    public class JwtTokenGenerator : IJwtTokenGenerator
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;

        public JwtTokenGenerator(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<string> GenerateTokenAsync(User user)
        {
            IList<Claim> claims = await GetClaimsAsync(user);
            JwtSecurityToken tokenOptions = GenerateTokenOptions(claims);

            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        private async Task<IList<Claim>> GetClaimsAsync(User user)
        {
            IList<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            IList<string> roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                var claim = new Claim(ClaimTypes.Role, role);
                claims.Add(claim);
            }

            return claims;
        }

        private JwtSecurityToken GenerateTokenOptions(IList<Claim> claims)
        {
            IConfigurationSection jwtSettings = _configuration.GetSection("JwtSettings");
            SigningCredentials signingCredentials = GetSigningCredentials(jwtSettings);

            var tokenOptions = new JwtSecurityToken
            (
                issuer: jwtSettings["validIssuer"],
                audience: jwtSettings["validAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["minutesToExpire"])),
                signingCredentials: signingCredentials
            );

            return tokenOptions;
        }

        private SigningCredentials GetSigningCredentials(IConfigurationSection jwtSettings)
        {
            string key = jwtSettings["secretKey"];
            if (key is null)
            {
                key = Environment.GetEnvironmentVariable("secretKey");
            }

            byte[] encodedKey = Encoding.UTF8.GetBytes(key);
            var securityKey = new SymmetricSecurityKey(encodedKey);

            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }
    }
}
