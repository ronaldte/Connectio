namespace Connectio.Models
{
    public class PostReactions
    {
        public int PostId { get; set; }
        public bool Bookmarked { get; set; } = false;

        public PostReactions(int postId)
        {
            PostId = postId;
        }
    }
}
