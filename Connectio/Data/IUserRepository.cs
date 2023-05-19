using Connectio.Models;

namespace Connectio.Data
{
    public interface IUserRepository
    {
        ApplicationUser? GetUserByUserName(string username);
        void UpdateFollower(ApplicationUser following);
        void UpdateProfilePicture(ApplicationUser user, string? fileName);
        void UpdateBannerPicture(ApplicationUser user, string? fileName);
    }
}
