using System.Security.Claims;

namespace api_login.Interfaces
{
    public interface IBasicValidations
    {
        bool IsAuthenticatedUser(ClaimsPrincipal? user = null);
        (bool Authenticated, bool Administrator) IsAdministratorUser(ClaimsPrincipal? user = null);
    }
}
