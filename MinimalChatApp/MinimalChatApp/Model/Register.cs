using System.ComponentModel.DataAnnotations;

namespace MinimalChatApplication.Model
{
    public class Register
    {
        [Required(ErrorMessage ="User name is Required")]
        public string? Name { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is Required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public string? Password { get; set; }
    }
}
