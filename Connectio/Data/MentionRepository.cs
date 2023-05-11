using Connectio.Models;

namespace Connectio.Data
{
    public class MentionRepository : IMentionRepository
    {
        public void AddTags(Post post)
        {
            throw new NotImplementedException();
        }

        public void AddUsers(Post post)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Post> GetPosts(Tag tag)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Post> GetPosts(ApplicationUser user)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tag> GetTags(Post post)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ApplicationUser> GetUsers(Post post)
        {
            throw new NotImplementedException();
        }
    }
}
