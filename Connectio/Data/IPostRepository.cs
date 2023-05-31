using Connectio.Models;

namespace Connectio.Data
{
    /// <summary>
    /// Post repository manages Posts.
    /// </summary>
    public interface IPostRepository
    {
        /// <summary>
        /// Adds new post to DB.
        /// </summary>
        /// <param name="post">Post to be added to DB.</param>
        void CreatePost(Post post);
        
        /// <summary>
        /// Gets post with given Id.
        /// </summary>
        /// <param name="postId">Post with Id to be returned.</param>
        /// <returns>Post with given Id.</returns>
        Post? GetPostById(int postId);
        
        /// <summary>
        /// Gets all posts created by user.
        /// </summary>
        /// <param name="username">Username of User whom posts are to be loaded.</param>
        /// <returns>List with all posts created by user.</returns>
        IEnumerable<Post> GetAllPostsByUser(string username);
        
        /// <summary>
        /// Gets all posts created on social media.
        /// </summary>
        /// <returns>List with all posts ever created.</returns>
        IEnumerable<Post> GetAllPosts();
    }
}
