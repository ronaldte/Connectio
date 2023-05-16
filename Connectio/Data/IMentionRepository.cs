using Connectio.Models;

namespace Connectio.Data
{
    public interface IMentionRepository
    {
        void AddTags(Post post);
        void AddUsers(Post post);
        Tag? GetTag(string tagName);
        IEnumerable<Post> GetPosts(Tag tag);
    }
}
