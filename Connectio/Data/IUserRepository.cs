using Connectio.Models;

namespace Connectio.Data
{
    /// <summary>
    /// User repository manages user profiles from database.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Get User model based on username.
        /// </summary>
        /// <param name="username">Username of User to be returned.</param>
        /// <returns>Application user; otherwise null if no such user exists.</returns>
        ApplicationUser? GetUserByUserName(string username);
        
        /// <summary>
        /// Saves new followers in user to DB.
        /// </summary>
        /// <param name="following">ApplicationUser with new followings.</param>
        void UpdateFollower(ApplicationUser following);
        
        /// <summary>
        /// Sets new user profile picture file name.
        /// </summary>
        /// <param name="user">User whos profile picture is updated.</param>
        /// <param name="fileName">Name of picture to be used as profile picture; or null for default picture.</param>
        void UpdateProfilePicture(ApplicationUser user, string? fileName);
        
        /// <summary>
        /// Sets new banner profile picture file name.
        /// </summary>
        /// <param name="user">User whos profile picture is updated.</param>
        /// <param name="fileName">Name of picture to be used as banner picture; or null for default picture.</param>
        void UpdateBannerPicture(ApplicationUser user, string? fileName);
    }
}
