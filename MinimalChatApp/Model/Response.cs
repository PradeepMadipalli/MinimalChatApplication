namespace MinimalChatApplication.Model
{
    public class Response
    {
        public string? Status { get; set; }
        public string? Message { get; set; }
    }
    public class MessageRequest
    {
        public string? ReceiverId { get; set; }
        public string? Content { get; set; }
    }
    public class EditMessageRequest
    {
        public string? Content { get; set; }
    }
    public class ConversationHistoryRequest
    {
        public string UserId { get; set; }
        public DateTime? Before { get; set; }
        public int Count { get; set; } = 20; // Default value: 20
        public string Sort { get; set; }
    }
}
