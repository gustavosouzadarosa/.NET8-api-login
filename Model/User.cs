using Microsoft.AspNetCore.Identity;

namespace ApiLogin.Model
{
    public class User : IdentityUser
    {
        public string? CampoAdicional { get; set; }
    }
}
