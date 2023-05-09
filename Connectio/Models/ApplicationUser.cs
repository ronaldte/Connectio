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
        public List<Post> Posts { get; set; } = new();
        public List<Post> BookmarkedPosts { get; set; } = new();
        public List<Bookmark> Bookmarks { get; set; } = new();

        public List<Post> LikedPosts { get; set; } = new();
        public List<Like> Likes { get; set; } = new();
    }
}
