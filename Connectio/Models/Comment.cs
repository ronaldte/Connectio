using System.ComponentModel.DataAnnotations;

namespace Connectio.Models
{
    /// <summary>
    /// Represents a text reply to a post.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Key for Comment.
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// Id of post which comment belongs to.
        /// </summary>
        public int PostId { get; set; }
        
        /// <summary>
        /// Post to which the comment belongs.
        /// </summary>
        public Post Post { get; set; } = null!;
        
        /// <summary>
        /// Id of user who made the comment.
        /// </summary>
        public string ApplicationUserId { get; set; } = null!;
        
        /// <summary>
        /// ApplicationUser who made the comment.
        /// </summary>
        public ApplicationUser User { get; set; } = null!;
        
        /// <summary>
        /// Body text content of the comment.
        /// </summary>
        public string Text { get; set; } = null!;
        
        /// <summary>
        /// UTC date and time when the comment was created.
        /// </summary>
        public DateTime Created { get; set; }
    }
}
