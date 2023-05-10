using Connectio.Models;
using Microsoft.EntityFrameworkCore;

namespace Connectio.Data
{
    public class PostRepository : IPostRepository
    {
        private readonly ConnectioDbContext _dbContext;

        public PostRepository(ConnectioDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void CreatePost(Post post)
        {
            _dbContext.Add(post);
            _dbContext.SaveChanges();
        }

        public IEnumerable<Post> GetAllPosts()
        {
            return _dbContext.Posts.Include(p => p.User);
        }

        public IEnumerable<Post> GetAllPostsByUser(string username)
        {
            return _dbContext.Posts.Where(p => p.User.UserName == username).Include(p => p.User);
        }

        public Post? GetPostById(int postId)
        {
            return _dbContext.Posts
                .Where(p => p.Id == postId)
                .Include(p => p.User)
                .Include(p => p.LikedBy)
                .Include(p => p.BookmarkedBy)
                .Include(p => p.Comments)
                .FirstOrDefault();
        }
    }
}
