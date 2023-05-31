using Connectio.Models;

namespace Connectio.Data
{
    /// <summary>
    /// Search repository manages searches on social media.
    /// </summary>
    public interface ISearchRepository
    {
        /// <summary>
        /// Gets Posts which contain searched keyword.
        /// </summary>
        /// <param name="searchKeyword">Search keyword to search for in posts.</param>
        /// <returns>List of Posts which contain searched keyword.</returns>
        IEnumerable<Post> GetPosts(string searchKeyword);
        
        /// <summary>
        /// Gets Users whos username or display name contains searched keyword.
        /// </summary>
        /// <param name="searchKeyword">Search keyword to search for in users.</param>
        /// <returns>List of ApplicationUsers whos name contains searched keyword.</returns>
        IEnumerable<ApplicationUser> GetUsers(string searchKeyword);
        
        /// <summary>
        /// Gets Tags which contain searched keyword.
        /// </summary>
        /// <param name="searchKeyword">Search keyword to search for in tags.</param>
        /// <returns>List of Tags which contain searched keyword.</returns>
        IEnumerable<Tag> GetTags(string searchKeyword);
    }
}
