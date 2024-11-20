using System.ComponentModel.DataAnnotations;

namespace ApiLogin.Model
{
    public class RegisterViewModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; } 

        public string? CampoAdicional { get; set; } // Campo personalizado
    }
}
