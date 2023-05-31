using Connectio.Models;

namespace Connectio.Data
{
    /// <summary>
    /// Mention repository manages parsing and creating mentions in posts.
    /// </summary>
    public interface IMentionRepository
    {
        /// <summary>
        /// Parses tags from post text.
        /// </summary>
        /// <param name="post">Post to be parsed.</param>
        void AddTags(Post post);
        
        /// <summary>
        /// Parses users from post text.
        /// </summary>
        /// <param name="post">Post to be parsed.</param>
        void AddUsers(Post post);
        
        /// <summary>
        /// Get tag model by tag name.
        /// </summary>
        /// <param name="tagName">Tag name to search for.</param>
        /// <returns>Tag is such tag exists, null otherwise.</returns>
        Tag? GetTag(string tagName);
        
        /// <summary>
        /// Gets all posts with given tag.
        /// </summary>
        /// <param name="tag">Tag used in posts.</param>
        /// <returns>List of posts tagged with given tag.</returns>
        IEnumerable<Post> GetPosts(Tag tag);
    }
}
