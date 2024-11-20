using Microsoft.AspNetCore.Identity;

namespace ApiLogin.Entities
{
    public class User : IdentityUser
    {
        public string? CampoAdicional { get; set; }
    }
}
