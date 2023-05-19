using Connectio.Models;

namespace Connectio.ViewModels
{
    public class ReadUserViewModel
    {
        public string UserName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string? Location { get; set; }
        public string? Url { get; set; }
        public string? Description { get; set; }
        public bool Protected { get; set; } = false;
        public bool Verified { get; set; } = false;
        public string Created { get; set; } = string.Empty;
        public List<ReadPostViewModel> Posts { get; set; } = new();
        public int FollowersCount { get; set; } = 0;
        public int FollowingCount { get; set; } = 0;
        public bool HasDefaultProfilePicture { get; set; } = true;
        public string ProfilePictureUrl { get; set; } = string.Empty;
        public bool HasDefaultBannerPicture { get; set; } = true;
        public string BannerPictureUrl { get; set; } = string.Empty;

        public ReadUserViewModel(ApplicationUser user)
        {
            UserName = user.UserName;
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
        public ReadUserViewModel(ApplicationUser user, List<ReadPostViewModel> posts) : this(user)
        {
            Posts = posts;
        }
    }
}
