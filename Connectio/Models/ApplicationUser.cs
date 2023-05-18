using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connectio.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string DisplayName { get; set; }
        public string? Location { get; set; }
        public string? Url { get; set; }
        public string? Description { get; set; }
        public bool Protected { get; set; } = false;
        public bool Verified { get; set; } = false;
        public DateTime Created { get; set; }
        public bool HasDefaultProfilePicture { get; set; } = true;
        public string ProfilePictureUrl { get; set; } = string.Empty;
        public bool HasDefaultBannerPicture { get; set; } = true;
        public string BannerPictureUrl { get; set; } = string.Empty;
        public List<Post> Posts { get; set; } = new();
        public List<Post> BookmarkedPosts { get; set; } = new();
        public List<Bookmark> Bookmarks { get; set; } = new();

        public List<Post> LikedPosts { get; set; } = new();
        public List<Like> Likes { get; set; } = new();

        public List<Post> CommentedPosts { get; set; } = new();
        public List<Comment> Comments { get; set; } = new();

        public List<ApplicationUser> Followers { get; } = new();
        public List<ApplicationUser> Following { get; } = new();

        public List<Post> PostMentions { get; } = new();
    }
}
