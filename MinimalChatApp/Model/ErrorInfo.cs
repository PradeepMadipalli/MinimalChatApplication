using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace MinimalChatApp.Model
{
    public class ErrorInfo
    {
        
     
        public int StatusCode { get; set; } = 0;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
