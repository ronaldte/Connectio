using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connectio.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime Created { get; set; }

        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        public List<ApplicationUser> BookmarkedBy { get; set; } = new();
        public List<Bookmark> Bookmarks { get; set; } = new();

        public List<ApplicationUser> LikedBy { get; set; } = new();
        public List<Like> Likes { get; set; } = new();

        public List<ApplicationUser> CommentedBy { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();

        public List<Tag> Tags { get; set; } = new();
        public List<ApplicationUser> UserMentions { get; set; } = new();
        public List<PostImage> PostImages { get; set; } = new();
    }
}
