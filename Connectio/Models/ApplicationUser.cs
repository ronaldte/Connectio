using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace Connectio.Models
{
    /// <summary>
    /// Represents unique users who interact with posts and other users.
    /// </summary>
    public class ApplicationUser : IdentityUser
    {
        /// <summary>
        /// Changable; short descriptive name of user.
        /// </summary>
        public string DisplayName { get; set; } = null!;
        
        /// <summary>
        /// Changable; location of user, does not have to be real.
        /// </summary>
        public string? Location { get; set; }
        
        /// <summary>
        /// Changable; users' website.
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Changable; users' short biography displayed on their profile page.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// If true users posts are only visible by people they follow.
        /// </summary>
        public bool Protected { get; set; } = false;

        /// <summary>
        /// If true persons profile is matching real person; prevents fradulant activity.
        /// </summary>
        public bool Verified { get; set; } = false;
        
        /// <summary>
        /// UTC date and time when the account was originally created.
        /// </summary>
        public DateTime Created { get; set; }

        /// <summary>
        /// Mark if user uses default profile picture consisting of letters from name or has custom uploaded picture.
        /// </summary>
        public bool HasDefaultProfilePicture { get; set; } = true;
        
        /// <summary>
        /// Address of user profile picture.
        /// </summary>
        public string ProfilePictureUrl { get; set; } = string.Empty;
        
        /// <summary>
        /// Mark if user uses default banner picture of gray color or has custom uploaded picture.
        /// </summary>
        public bool HasDefaultBannerPicture { get; set; } = true;
        
        /// <summary>
        /// Address of user banner picture.
        /// </summary>
        public string BannerPictureUrl { get; set; } = string.Empty;
        
        /// <summary>
        /// Posts created by user.
        /// </summary>
        public List<Post> Posts { get; set; } = new();
        
        /// <summary>
        /// Posts bookmarked by user.
        /// </summary>
        public List<Post> BookmarkedPosts { get; set; } = new();
        
        /// <summary>
        /// Bookmarks created by user.
        /// </summary>
        public List<Bookmark> Bookmarks { get; set; } = new();

        
        /// <summary>
        /// Posts liked by user.
        /// </summary>
        public List<Post> LikedPosts { get; set; } = new();
        
        /// <summary>
        /// Likes created by user.
        /// </summary>
        public List<Like> Likes { get; set; } = new();

        /// <summary>
        /// Posts which user commented on.
        /// </summary>
        public List<Post> CommentedPosts { get; set; } = new();
        
        /// <summary>
        /// Comments created by user.
        /// </summary>
        public List<Comment> Comments { get; set; } = new();

        /// <summary>
        /// Other users who follow this user.
        /// </summary>
        public List<ApplicationUser> Followers { get; } = new();
        
        /// <summary>
        /// Other users who are followed by this user.
        /// </summary>
        public List<ApplicationUser> Following { get; } = new();

        /// <summary>
        /// Posts where user is mentioned.
        /// </summary>
        public List<Post> PostMentions { get; } = new();

        /// <summary>
        /// Conversations where user participates.
        /// </summary>
        public List<Conversation> Conversations { get; set; } = new();
    }
}
