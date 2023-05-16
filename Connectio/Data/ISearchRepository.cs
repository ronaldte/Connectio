using Connectio.Models;

namespace Connectio.Data
{
    public interface ISearchRepository
    {
        IEnumerable<Post> GetPosts(string searchKeyword);
        IEnumerable<ApplicationUser> GetUsers(string searchKeyword);
        IEnumerable<Tag> GetTags(string searchKeyword);
    }
}
