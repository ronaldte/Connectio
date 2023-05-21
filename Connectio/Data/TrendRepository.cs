using Connectio.Models;
using Microsoft.EntityFrameworkCore;

namespace Connectio.Data
{
    public class TrendRepository : ITrendRepository
    {
        private readonly ConnectioDbContext _dbContext;

        public TrendRepository(ConnectioDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<ApplicationUser> GetPeopleToFollow(ApplicationUser user, int numberOfUsers = 3)
        {
            return _dbContext.Users
                .Where(u => u != user && !u.Followers.Contains(user))
                .OrderBy(g => Guid.NewGuid())
                .Take(numberOfUsers)
                .ToList();
        }

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
