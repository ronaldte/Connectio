using Connectio.Models;

namespace Connectio.ViewModels
{
    /// <summary>
    /// ReadReaction model represents reactions status on the post.
    /// </summary>
    public class ReadReactionViewModel
    {
        /// <summary>
        /// Post of which reactions are gathered.
        /// </summary>
        public int PostId { get; set; }
        
        /// <summary>
        /// Number of comments on the post.
        /// </summary>
        public int CommentsCount { get; set; } = 0;
        
        /// <summary>
        /// Number of likes on the post.
        /// </summary>
        public int LikesCount { get; set; } = 0;
        
        /// <summary>
        /// Number of bookmarks on the post.
        /// </summary>
        public int BookmarksCount { get; set; } = 0;
        
        /// <summary>
        /// State if the post is liked or not.
        /// </summary>
        public bool Liked { get; set; } = false;
        
        /// <summary>
        /// State if the post is bookmarked or not.
        /// </summary>
        public bool Bookmarked { get; set; } = false;
    }
}
