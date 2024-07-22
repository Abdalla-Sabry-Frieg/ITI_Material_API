using System.ComponentModel.DataAnnotations;

namespace ITI_Material.DTOs
{
    public class RegisterUserDto
    {
        [Required]
        public string userName  { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        [Compare("Password")]
        public string comparePassword { get; set; }
        public string Email { get; set; }
    }
}
