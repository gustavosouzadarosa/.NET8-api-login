using System.ComponentModel.DataAnnotations;

namespace ApiLogin.Model
{
    public class EditUserViewModel
    {
        [Required]
        public required string UserId { get; set; }
        public string? CampoAdicional { get; set; }
    }
}
