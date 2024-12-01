using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ApiLogin.Interfaces;
using ApiLogin.Model;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace ApiLogin.Implementations
{
    public class JwtMiddleware : IJwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly TokenSettings _tokenSettings;

        public JwtMiddleware(RequestDelegate next, IOptions<TokenSettings> tokenSettings)
        {
            _next = next;
            _tokenSettings = tokenSettings.Value;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                var headerToken = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();

                if (headerToken != null)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_tokenSettings.Secret);

                    try
                    {
                        tokenHandler.ValidateToken(headerToken, new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey(key),
                            ValidateIssuer = true,
                            ValidIssuer = _tokenSettings.Issuer,
                            ValidateAudience = true,
                            ValidAudience = _tokenSettings.Audience, 
                            ClockSkew = TimeSpan.Zero 
                        }, out SecurityToken validatedToken);

                        JwtSecurityToken? token = tokenHandler.ReadToken(headerToken) as JwtSecurityToken;

                        if (token != null)
                        {
                            var userId = token.Claims.FirstOrDefault(c => c.Type.Trim().ToUpper() == "NAMEID");                  
                            var roles = token.Claims.Where(c => c.Type.Trim().ToUpper() == "ROLE").ToList();

                            var claimsIdentity = new ClaimsIdentity(null, "Basic");
                            if (userId != null)
                            {
                                claimsIdentity.AddClaim(userId);                      
                            }
                            claimsIdentity.AddClaims(roles);

                            var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);                           
                            context.User = claimsPrincipal;                            

                            context.Items["MoreInformation"] = "MoreInformation"; 
                        }
                        else
                        {
                            context.Response.StatusCode = 401;
                            await context.Response.WriteAsync("Invalid Token. (ERROR 0006) ");
                            return;
                        }
                    }
                    catch (SecurityTokenExpiredException ex)
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync($"Token expired. Please re-authenticate. Token expired on: {ex.Message}. (ERROR 0004) ");
                        return;
                    }
                    catch (SecurityTokenException)
                    {
                        context.Response.StatusCode = 401;
                        await context.Response.WriteAsync($"Invalid Token. (ERROR 0005) ");
                        return;
                    }
                }
                else
                {
                    // The "headerToken" variable can be null, permission validation will occur directly in the endpoint controller methods.
                }
            }
            catch (Exception)
            {
                context.Response.StatusCode = 500;
                await context.Response.WriteAsync("Internal Server Error. (ERROR 0003) ");
                return;
            }

            await _next(context);
        }
    }
}