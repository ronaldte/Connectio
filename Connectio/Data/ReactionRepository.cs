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

        public Like? GetLike(ApplicationUser user, Post post)
        {
            return _dbContext.Likes.Where(b => b.User == user && b.Post == post).FirstOrDefault();
        }

        public IEnumerable<Like> GetAllUserLikes(ApplicationUser user)
        {
            return _dbContext.Likes.Where(b => b.User == user).Include(b => b.Post);
        }

        public void CreateLike(Like like)
        {
            _dbContext.Likes.Add(like);
            _dbContext.SaveChanges();
        }

        public void DeleteLike(Like like)
        {
            _dbContext.Likes.Remove(like);
            _dbContext.SaveChanges();
        }
        
        public PostReactions GetReactions(ApplicationUser user, Post post)
        {
            return new PostReactions(post)
            {
                Bookmarked = _dbContext.Bookmarks.Any(b => b.User == user && b.Post == post),
                Liked = _dbContext.Likes.Any(b => b.User == user && b.Post == post)
            };
        }

        public void CreateComment(Comment comment)
        {
            _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();
        }

        public void DeleteComment(Comment comment)
        {
            _dbContext.Comments.Remove(comment);
            _dbContext.SaveChanges();
        }

        public List<Comment> GetAllCommentsOnPost(Post post)
        {
            return _dbContext.Comments.Where(c => c.Post == post).Include(c => c.User).ToList();
        }

        public IEnumerable<Comment> GetAllUserComments(ApplicationUser user)
        {
            return _dbContext.Comments.Where(c => c.User == user).Include(c => c.Post);
        }
    }
}
