namespace Connectio.Models
{
    /// <summary>
    /// Represents user appreciation on the post.
    /// </summary>
    public class Like
    {
        /// <summary>
        /// Id of post which user liked.
        /// </summary>
        public int PostId { get; set; }
        
        /// <summary>
        /// Post which user liked.
        /// </summary>
        public Post Post { get; set; } = null!;
        
        /// <summary>
        /// Id of user who liked the post.
        /// </summary>
        public string ApplicationUserId { get; set; } = null!;
        
        /// <summary>
        /// ApplicationUser who liked the post.
        /// </summary>
        public ApplicationUser User { get; set; } = null!;
        
        /// <summary>
        /// UTC date and time when the user liked the post.
        /// </summary>
        public DateTime Created { get; set; }
    }
}
