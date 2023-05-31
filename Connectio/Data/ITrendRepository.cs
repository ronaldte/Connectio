using Connectio.Models;

namespace Connectio.Data
{
    /// <summary>
    /// Trend repository manages popular topics and users on social media.
    /// </summary>
    public interface ITrendRepository
    {
        /// <summary>
        /// Gets people to follow.
        /// </summary>
        /// <param name="user">User for whom to display people to follow.</param>
        /// <param name="numberOfUsers">Number of entries of people to follow.</param>
        /// <returns>List of ApplicationUser of whom user may be interested to follow.</returns>
        IEnumerable<ApplicationUser> GetPeopleToFollow(ApplicationUser user, int numberOfUsers = 3);
        
        /// <summary>
        /// Gets trending tags.
        /// </summary>
        /// <param name="numberOfTags">Number of tags to return.</param>
        /// <returns>List of Tag that are currenlty trending of social media.</returns>
        IEnumerable<Tag> GetTrendingTags(int numberOfTags = 5);
    }
}
