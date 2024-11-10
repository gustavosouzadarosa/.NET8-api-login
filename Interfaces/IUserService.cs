using System.Security.Claims;

namespace api_login.Interfaces
{
    public interface IUserService
    {
        ClaimsPrincipal User { get; }
    }
}
