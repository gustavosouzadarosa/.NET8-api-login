using System.Security.Claims;

namespace ApiLogin.Interfaces
{
    public interface IUserService
    {
        ClaimsPrincipal User { get; }
    }
}