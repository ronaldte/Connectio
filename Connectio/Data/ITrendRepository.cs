using Connectio.Models;

namespace Connectio.Data
{
    public interface ITrendRepository
    {
        IEnumerable<ApplicationUser> GetPeopleToFollow(ApplicationUser user, int numberOfUsers = 3);
        IEnumerable<Tag> GetTrendingTags(int numberOfTags = 5);
    }
}
