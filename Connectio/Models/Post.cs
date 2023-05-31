using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connectio.Models
{
    /// <summary>
    /// Represents the main way of sharing a thought in text form.
    /// </summary>
    public class Post
    {
        /// <summary>
        /// Key of Post.
        /// </summary>
        [Key]
        public int Id { get; set; }
        
        /// <summary>
        /// Body text content of a post.
        /// </summary>
        public string Text { get; set; } = string.Empty;
        
        /// <summary>
        /// UTC date and time when was the post created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Id of user who created the Post.
        /// </summary>
        public string UserId { get; set; } = null!;
        
        /// <summary>
        /// APplicationUser who created the post.
        /// </summary>
        public ApplicationUser User { get; set; } = null!;

        /// <summary>
        /// ApplicationUsers who bookmarked the post.
        /// </summary>
        public List<ApplicationUser> BookmarkedBy { get; set; } = new();
        
        /// <summary>
        /// Bookmarks in which the post is bookmarked.
        /// </summary>
        public List<Bookmark> Bookmarks { get; set; } = new();

        /// <summary>
        /// ApplicationUsers who liked the post.
        /// </summary>
        public List<ApplicationUser> LikedBy { get; set; } = new();
        
        /// <summary>
        /// Likes in which is the post liked.
        /// </summary>
        public List<Like> Likes { get; set; } = new();

        /// <summary>
        /// ApplicationUsers who made an comments on the post.
        /// </summary>
        public List<ApplicationUser> CommentedBy { get; set; } = new();
        
        /// <summary>
        /// Comments in which is the post commented on.
        /// </summary>
        public List<Comment> Comments { get; set; } = new();

        /// <summary>
        /// Tags which are mentioned in the post.
        /// </summary>
        public List<Tag> Tags { get; set; } = new();
        
        /// <summary>
        /// ApplicationUsers who are mentioned in the post.
        /// </summary>
        public List<ApplicationUser> UserMentions { get; set; } = new();
        
        /// <summary>
        /// Images which were added to the post body.
        /// </summary>
        public List<PostImage> PostImages { get; set; } = new();
    }
}
