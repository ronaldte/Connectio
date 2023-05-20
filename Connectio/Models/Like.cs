namespace Connectio.Models
{
    public class Like
    {
        public int PostId { get; set; }
        public Post Post { get; set; } = null!;
        public string ApplicationUserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public DateTime Created { get; set; }
    }
}
