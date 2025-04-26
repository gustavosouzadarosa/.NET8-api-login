using Microsoft.AspNetCore.Identity;

namespace ApiLogin.Entities
{
    public class User : IdentityUser
    {
        public string? CampoAdicional { get; set; }
        public string? CampoAdicional2 { get; set; }
    }
}
