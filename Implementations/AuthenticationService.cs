using ApiLogin.Custom;
using ApiLogin.Entities;
using ApiLogin.Interfaces;
using ApiLogin.Model;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ApiLogin.Implementations
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<User> _userManager;
        private readonly TokenSettings _tokenSettings;

        public AuthenticationService(UserManager<User> userManager, IOptions<TokenSettings> tokenSettings)
        {
            _userManager = userManager;
            _tokenSettings = tokenSettings.Value;
        }

        public async Task<string> GenerateToken(string username, string password)
        {            
            string tokenString = "";

            try
            {
                var user = await _userManager.FindByNameAsync(username);

                if (user == null || !await _userManager.CheckPasswordAsync(user, password))
                {
                    return "Invalid username or password.";
                }

                var userRoles = await _userManager.GetRolesAsync(user);

                List<Claim> claims = [new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())];
                if (!string.IsNullOrWhiteSpace(user.UserName)) claims.Add(new Claim(ClaimTypes.Name, user.UserName));
                if (userRoles.Contains("Administrator")) claims.Add(new Claim(ClaimTypes.Role, "Administrator"));

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSettings.Secret));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(claims),
                    Issuer = _tokenSettings.Issuer,
                    Audience = _tokenSettings.Audience,
                    Expires = DateTime.UtcNow.AddMinutes(_tokenSettings.ExpirationMinutes > 0 ? _tokenSettings.ExpirationMinutes : 30),
                    SigningCredentials = creds
                };

                var tokenHandler = new JwtSecurityTokenHandler();
                var token = tokenHandler.CreateToken(tokenDescriptor);
                tokenString = tokenHandler.WriteToken(token);
            }
            catch (Exception ex)
            {
                throw new TokenGenerationException("Error generating token. (ERROR 0002)", ex);
            }

            return tokenString;
        }
    }
}