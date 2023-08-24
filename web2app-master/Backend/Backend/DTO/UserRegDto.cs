using System.ComponentModel.DataAnnotations;

namespace Backend.DTO
{
    public class UserRegDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string Address { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Picture { get; set; }
        [Required]
        [RegularExpression("^(Customer|Seller)$", ErrorMessage = "Role must be either 'Customer' or 'Seller'.")]
        public string Role { get; set; }
    }
}
