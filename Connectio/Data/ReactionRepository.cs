using Connectio.Models;
using Microsoft.EntityFrameworkCore;

namespace Connectio.Data
{
    /// <inheritdoc/>
    public class ReactionRepository : IReactionRepository
    {
        private readonly ConnectioDbContext _dbContext;

        public ReactionRepository(ConnectioDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc/>
        public Bookmark? GetBookmark(ApplicationUser user, Post post)
        {
            return _dbContext.Bookmarks.Where(b => b.User == user && b.Post == post).FirstOrDefault();
        }

        /// <inheritdoc/>
        public void CreateBookmark(Bookmark bookmark)
        {
            _dbContext.Bookmarks.Add(bookmark);
            _dbContext.SaveChanges();
        }

        /// <inheritdoc/>
        public void DeleteBookmark(Bookmark bookmark)
        {
            _dbContext.Bookmarks.Remove(bookmark);
            _dbContext.SaveChanges();
        }

        /// <inheritdoc/>
        public List<Bookmark> GetAllBookmarks(ApplicationUser user)
        {
            return _dbContext.Bookmarks.Where(b => b.User == user).Include(b => b.Post).Include(p => p.Post.User).Include(b => b.Post.PostImages).ToList();
        }

        /// <inheritdoc/>
        public Like? GetLike(ApplicationUser user, Post post)
        {
            return _dbContext.Likes.Where(b => b.User == user && b.Post == post).FirstOrDefault();
        }

        /// <inheritdoc/>
        public IEnumerable<Like> GetAllUserLikes(ApplicationUser user)
        {
            return _dbContext.Likes.Where(b => b.User == user).Include(b => b.Post).ThenInclude(p => p.User).ToList();
        }

        /// <inheritdoc/>
        public void CreateLike(Like like)
        {
            _dbContext.Likes.Add(like);
            _dbContext.SaveChanges();
        }

        /// <inheritdoc/>
        public void DeleteLike(Like like)
        {
            _dbContext.Likes.Remove(like);
            _dbContext.SaveChanges();
        }

        /// <inheritdoc/>
        public void CreateComment(Comment comment)
        {
            _dbContext.Comments.Add(comment);
            _dbContext.SaveChanges();
        }

        /// <inheritdoc/>
        public void DeleteComment(Comment comment)
        {
            _dbContext.Comments.Remove(comment);
            _dbContext.SaveChanges();
        }

        /// <inheritdoc/>
        public List<Comment> GetAllCommentsOnPost(Post post)
        {
            return _dbContext.Comments.Where(c => c.Post == post).Include(c => c.User).ToList();
        }

        /// <inheritdoc/>
        public IEnumerable<Comment> GetAllUserComments(ApplicationUser user)
        {
            return _dbContext.Comments.Where(c => c.User == user).Include(c => c.Post).ThenInclude(p => p.User).ToList();
        }
    }
}
