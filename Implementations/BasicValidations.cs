using System.Security.Claims;
using api_login.Interfaces;

namespace api_login.Implementations
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
            if (user == null) user = _userService.User;

            if (user == null || user.Identity == null || !user.Identity.IsAuthenticated)
            {
                return false;
            }

            return true;
        }

        public (bool Authenticated, bool Administrator) IsAdministratorUser(ClaimsPrincipal? user = null)
        {
            if (user == null) user = _userService.User;

            bool authenticated = IsAuthenticatedUser();
            bool administrator = user.IsInRole("Administrator");
         
            return (Authenticated: authenticated, Administrator: administrator);
        }
    }
}
