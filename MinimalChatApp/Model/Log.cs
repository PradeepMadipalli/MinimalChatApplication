using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalChatApp.Model
{
    public class Log
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string IpAddress { get; set; }
        public string RequestBody { get; set; }
        public DateTime RequestTime { get; set; }
        public string Username { get; set; }
        
    }
}
