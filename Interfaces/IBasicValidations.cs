using System.Security.Claims;

namespace ApiLogin.Interfaces
{
    public interface IBasicValidations
    {
        bool IsAuthenticatedUser(ClaimsPrincipal? user = null);
        (bool Authenticated, bool Administrator) IsAdministratorUser(ClaimsPrincipal? user = null);
    }
}