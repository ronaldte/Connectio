namespace Connectio.Models
{
    /// <summary>
    /// Represents posts which user saved for latter.
    /// </summary>
    public class Bookmark
    {
        /// <summary>
        /// Post Id of bookmarked post.
        /// </summary>
        public int PostId { get; set; }
        
        /// <summary>
        /// Post which is bookmarked.
        /// </summary>
        public Post Post { get; set; } = null!;
        
        /// <summary>
        /// User Id who bookmarked the post.
        /// </summary>
        public string ApplicationUserId { get; set; } = null!;
        
        /// <summary>
        /// ApplicationUser who bookmarked the post.
        /// </summary>
        public ApplicationUser User { get; set; } = null!;
        
        /// <summary>
        /// UTC date and time when the bookmark was created.
        /// </summary>
        public DateTime Created { get; set; }
    }
}
