namespace Connectio.Models
{
    public class Like
    {
        public int PostId { get; set; }
        public Post Post { get; set; }
        public string ApplicationUserId { get; set; }
        public ApplicationUser User { get; set; }
        public DateTime Created { get; set; }
    }
}
