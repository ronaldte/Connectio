using Connectio.Models;
using Microsoft.EntityFrameworkCore;

namespace Connectio.Data
{
    public class ReactionRepository : IReactionRepository
    {
        private readonly ConnectioDbContext _dbContext;

        public ReactionRepository(ConnectioDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Bookmark? GetBookmark(ApplicationUser user, Post post)
        {
            return _dbContext.Bookmarks.Where(b => b.User == user && b.Post == post).FirstOrDefault();
        }

        public void CreateBookmark(Bookmark bookmark)
        {
            _dbContext.Bookmarks.Add(bookmark);
            _dbContext.SaveChanges();
        }

        public void DeleteBookmark(Bookmark bookmark)
        {
            _dbContext.Bookmarks.Remove(bookmark);
            _dbContext.SaveChanges();
        }

        public List<Bookmark> GetAllBookmarks(ApplicationUser user)
        {
            return _dbContext.Bookmarks.Where(b => b.User == user).Include(b => b.Post).Include(p => p.Post.User).ToList();
        }

        public PostReactions GetReactions(ApplicationUser user, Post post)
        {
            return new PostReactions(post.Id)
            {
                Bookmarked = _dbContext.Bookmarks.Any(b => b.User == user && b.Post == post)
            };
        }
    }
}
