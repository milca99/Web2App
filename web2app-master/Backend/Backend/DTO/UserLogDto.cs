﻿using System.ComponentModel.DataAnnotations;

namespace Backend.DTO
{
    public class UserLogDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
