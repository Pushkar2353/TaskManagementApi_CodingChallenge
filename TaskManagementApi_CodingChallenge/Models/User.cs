using System.ComponentModel.DataAnnotations;

namespace TaskManagementApi_CodingChallenge.Models
{
    public class User
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(15, ErrorMessage = "Username must not exceed 15 characters.")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "The Password is required.")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "The Password must be between 8 and 20 characters.")]
        [RegularExpression("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[@$!%*?&])[A-Za-z\\d@$!%*?&]{8,}$", ErrorMessage = "The Password must have at least one uppercase letter, one lowercase letter, one digit, and one special character.")]
        public string Password { get; set; }= string.Empty;
    }
}
