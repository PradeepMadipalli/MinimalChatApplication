using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalChatApp.Model
{
    public class ErrorLogger
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int LoggerId { get; set; }
        [Required]
        public string ErrorDetails { get; set; } = String.Empty;
        public DateTime LogDate { get; set; } = DateTime.Now;
    }
}
