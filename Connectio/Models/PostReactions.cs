namespace Connectio.Models
{
    /// <summary>
    /// Represents all types of reactions on single post.
    /// </summary>
    public class PostReactions
    {
        /// <summary>
        /// Id of post to which reactions belong.
        /// </summary>
        public int PostId { get; set; }
        
        /// <summary>
        /// If Post is bookmarked by user.
        /// </summary>
        public bool Bookmarked { get; set; } = false;
        
        /// <summary>
        /// If Post is liked by user.
        /// </summary>
        public bool Liked { get; set; } = false;
        
        /// <summary>
        /// Numbers of likes on the post.
        /// </summary>
        public int LikedCount { get; set; } = 0;
        
        /// <summary>
        /// Number of bookmarks made with the post.
        /// </summary>
        public int BookmarkedCount { get; set; } = 0;
        
        /// <summary>
        /// Number of comments on the post.
        /// </summary>
        public int CommentedCount { get; set; } = 0;

        public PostReactions(Post post)
        {
            PostId = post.Id;
            LikedCount = post.LikedBy.Count;
            BookmarkedCount = post.BookmarkedBy.Count;
            CommentedCount = post.Comments.Count;
        }
    }
}
