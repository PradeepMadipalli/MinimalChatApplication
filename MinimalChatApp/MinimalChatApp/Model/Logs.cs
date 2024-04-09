using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MinimalChatApp.Model
{
    public class Logs
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }

        public string? Ipaddress { get; set; }
        [MaxLength(int.MinValue)]
        public string? RequestBody { get; set; }
      
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
    }
}
