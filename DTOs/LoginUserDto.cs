using System.ComponentModel.DataAnnotations;

namespace UrlShortener.DTOs
{
    public class LoginUserDto
    {
        [Required]
        [MaxLength(20)]
        public string Username { get; set; }

        [Required]
        [MinLength(4)]
        public string Password { get; set; }
    }
}