using Connectio.Models;

namespace Connectio.Data
{
    public interface IPostRepository
    {
        void CreatePost(Post post);
        Post? GetPostById(int postId);
        IEnumerable<Post> GetAllPosts();
    }
}
