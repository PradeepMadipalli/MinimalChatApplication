using System.ComponentModel.DataAnnotations;

namespace MinimalChatApplication.Model
{
    public class Message
    {
        [Required]
        public string? MessageId { get; set; }
        [Required]
        public string? SenderId { get; set; }
        [Required]
        public string? ReceiverId { get; set; }
        [Required]
        public string? Content { get; set; }
        [Required]
        public DateTime Timestamp { get; set; }

    }
}
