using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApiLogin.Model
{
    public class CreateUserViewModel
    {
        [Required]
        public required string UserName { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        public required string Password { get; set; }

        public string? CampoAdicional { get; set; }

        [DefaultValue(false)]
        public bool IsAdmin { get; set; } = false; // This variable exists only for development and test environments. In a production environment, the first admin user must be created in another way.
    }
}
