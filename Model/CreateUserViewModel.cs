﻿using System.ComponentModel.DataAnnotations;

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
    }
}
