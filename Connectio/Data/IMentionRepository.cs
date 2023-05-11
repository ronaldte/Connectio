using Connectio.Models;

namespace Connectio.Data
{
    public interface IMentionRepository
    {
        void AddTags(Post post);
        void AddUsers(Post post);
        IEnumerable<Tag> GetTags(Post post);
        IEnumerable<ApplicationUser> GetUsers(Post post);
        IEnumerable<Post> GetPosts(Tag tag);
        IEnumerable<Post> GetPosts(ApplicationUser user);
    }
}
