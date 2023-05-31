using Connectio.Models;
using Microsoft.EntityFrameworkCore;

namespace Connectio.Data
{
    /// <inheritdoc/>
    public class SearchRepository : ISearchRepository
    {
        private readonly ConnectioDbContext _dbContext;

        public SearchRepository(ConnectioDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc/>
        public IEnumerable<Post> GetPosts(string searchKeyword)
        {
            return _dbContext.Posts.Where(p => p.Text.Contains(searchKeyword)).Include(p => p.User).ToList();
        }

        /// <inheritdoc/>
        public IEnumerable<ApplicationUser> GetUsers(string searchKeyword)
        {
            return _dbContext.Users.Where(u => u.UserName!.Contains(searchKeyword) || u.DisplayName!.Contains(searchKeyword)).ToList();
        }

        /// <inheritdoc/>
        public IEnumerable<Tag> GetTags(string searchKeyword)
        {
            return _dbContext.Tags.Where(t => t.Name.Contains(searchKeyword)).Include(t => t.Posts).ToList();
        }
    }
}
