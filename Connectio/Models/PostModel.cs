using System.ComponentModel.DataAnnotations;

namespace Connectio.Models
{
    public class PostModel
    {
        [Key]
        public int Id { get; set; }
        
        public string Text { get; set; } = string.Empty;
    }
}
