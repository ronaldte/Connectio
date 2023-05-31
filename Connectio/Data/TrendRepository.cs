using Connectio.Models;
using Microsoft.EntityFrameworkCore;

namespace Connectio.Data
{
    /// <inheritdoc/>
    public class TrendRepository : ITrendRepository
    {
        private readonly ConnectioDbContext _dbContext;

        public TrendRepository(ConnectioDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc/>
        public IEnumerable<ApplicationUser> GetPeopleToFollow(ApplicationUser user, int numberOfUsers = 3)
        {
            return _dbContext.Users
                .Where(u => u != user && !u.Followers.Contains(user))
                .OrderBy(g => Guid.NewGuid())   // use simple random order of people; generate Guid for each user and then order based on it
                .Take(numberOfUsers)
                .ToList();
        }

        /// <inheritdoc/>
        public IEnumerable<Tag> GetTrendingTags(int numberOfTags = 5)
        {
            return _dbContext.Tags
                .OrderByDescending(t => t.Posts.Count)
                .Take(numberOfTags)
                .Include(t => t.Posts)
                .ToList();
        }
    }
}
