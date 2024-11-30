using System.ComponentModel.DataAnnotations;

namespace ApiLogin.Model
{
    public class LoginViewModel
    {
        [Required]
        public required string UserName { get; set; }

        [Required]
        public required string Password { get; set; }

    }
}
