using System.ComponentModel.DataAnnotations;

namespace Connectio.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        
        public string Text { get; set; } = string.Empty;

        public DateTime Created { get; set; }
    }
}
