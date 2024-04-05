using System.ComponentModel.DataAnnotations;

namespace MinimalChatApplication.Model
{
    public class Users
    {
        [Required(ErrorMessage ="Enter Email")]
        public string? Email {  get; set; }
        [Required(ErrorMessage ="Enter Password")]
        public string? Password { get; set; }
    }
}
