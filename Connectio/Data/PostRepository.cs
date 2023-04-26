using Connectio.Models;

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
            return _dbContext.Posts;
        }

        public Post? GetPostById(int postId)
        {
            return _dbContext.Posts.Where(p => p.Id == postId).FirstOrDefault();
        }
    }
}
