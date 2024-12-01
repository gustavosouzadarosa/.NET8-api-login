using ApiLogin.Custom;
using ApiLogin.Interfaces;
using System.Security.Claims;

namespace ApiLogin.Implementations
{
    public class BasicValidations : IBasicValidations
    {
        private readonly IUserService _userService;

        public BasicValidations(IUserService userService)
        {
            _userService = userService;
        }

        public bool IsAuthenticatedUser(ClaimsPrincipal? user = null)
        {
            try
            {
                if (user == null) user = _userService.User;

                if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                throw new GenericApplicationException("Error authenticating user. (ERROR 0007)", ex);
            }
        }

        public (bool Authenticated, bool Administrator) IsAdministratorUser(ClaimsPrincipal? user = null)
        {
            try
            {
                if (user == null) user = _userService.User;

                bool authenticated = IsAuthenticatedUser();
                bool administrator = user.Claims.Any(x => x.Type == "role" && x.Value.Trim().ToUpper() == "ADMINISTRATOR");

                return (Authenticated: authenticated, Administrator: administrator);
            }
            catch (Exception ex)
            {
                throw new GenericApplicationException("Error authorizing user. (ERROR 0008)", ex);
            }
        }
    }
}