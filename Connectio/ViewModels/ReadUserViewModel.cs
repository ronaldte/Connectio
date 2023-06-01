using Connectio.Models;

namespace Connectio.ViewModels
{
    /// <summary>
    /// Readuser model represents user profile entity.
    /// </summary>
    public class ReadUserViewModel
    {
        /// <summary>
        /// Unique username of the user profile.
        /// </summary>
        public string UserName { get; set; } = string.Empty;
        
        /// <summary>
        /// Short descriptive name of the use.
        /// </summary>
        public string DisplayName { get; set; } = string.Empty;
        
        /// <summary>
        /// Location of the user.
        /// </summary>
        public string? Location { get; set; }
        
        /// <summary>
        /// Url of the user.
        /// </summary>
        public string? Url { get; set; }
        
        /// <summary>
        /// Short biography of user.
        /// </summary>
        public string? Description { get; set; }
        
        /// <summary>
        /// State if users' posts are publicly visible.
        /// </summary>
        public bool Protected { get; set; } = false;
        
        /// <summary>
        /// State if user profile matches real person.
        /// </summary>
        public bool Verified { get; set; } = false;
        
        /// <summary>
        /// Date when was the user account created.
        /// </summary>
        public string Created { get; set; } = string.Empty;
        
        /// <summary>
        /// List of all user posts.
        /// </summary>
        public IEnumerable<ReadPostViewModel> Posts { get; set; } = Enumerable.Empty<ReadPostViewModel>();
        
        /// <summary>
        /// Number of followers user account has.
        /// </summary>
        public int FollowersCount { get; set; } = 0;
        
        /// <summary>
        /// Number of users user account follows.
        /// </summary>
        public int FollowingCount { get; set; } = 0;
        
        /// <summary>
        /// State if user uses default latter picture or has custom one.
        /// </summary>
        public bool HasDefaultProfilePicture { get; set; } = true;
        
        /// <summary>
        /// Location of custom profile picture.
        /// </summary>
        public string ProfilePictureUrl { get; set; } = string.Empty;
        
        /// <summary>
        /// State if user uses default banner picture or has custom one.
        /// </summary>
        public bool HasDefaultBannerPicture { get; set; } = true;
        
        /// <summary>
        /// Location of custom banner picture.
        /// </summary>
        public string BannerPictureUrl { get; set; } = string.Empty;

        public ReadUserViewModel(ApplicationUser user)
        {
            UserName = user.UserName!;
            DisplayName = user.DisplayName;
            Location = user.Location;
            Url = user.Url;
            Description = user.Description;
            Protected = user.Protected;
            Verified = user.Verified;
            Created = user.Created.ToString("Y");
            FollowersCount = user.Followers.Count;
            FollowingCount = user.Following.Count;

            if (!user.HasDefaultBannerPicture)
            {
                HasDefaultBannerPicture = false;
                BannerPictureUrl = user.BannerPictureUrl;
            }
            if (!user.HasDefaultProfilePicture)
            {
                HasDefaultProfilePicture = false;
                ProfilePictureUrl = user.ProfilePictureUrl;
            }
        }
        public ReadUserViewModel(ApplicationUser user, IEnumerable<ReadPostViewModel> posts) : this(user)
        {
            Posts = posts;
        }
    }
}
