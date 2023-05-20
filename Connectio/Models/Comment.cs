using System.ComponentModel.DataAnnotations;

namespace Connectio.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; } = null!;
        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public string Text { get; set; } = null!;
        public DateTime Created { get; set; }
    }
}
