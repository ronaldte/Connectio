namespace Connectio.Models
{
    public class PostReactions
    {
        public int PostId { get; set; }
        public bool Bookmarked { get; set; } = false;
        public bool Liked { get; set; } = false;
        public int LikedCount { get; set; } = 0;
        public int BookmarkedCount { get; set; } = 0;

        public PostReactions(Post post)
        {
            PostId = post.Id;
            LikedCount = post.LikedBy.Count;
            BookmarkedCount = post.BookmarkedBy.Count;
        }
    }
}
