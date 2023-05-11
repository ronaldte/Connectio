using Connectio.Models;

namespace Connectio.Data
{
    public class MentionRepository : IMentionRepository
    {
        
        private readonly ConnectioDbContext _dbContext;

        public MentionRepository(ConnectioDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        private Post AddTag(string tagname, Post post)
        {
            var tag = _dbContext.Tags.Where(t => t.Name == tagname).FirstOrDefault();
            if(tag == null)
            {
                tag = new Tag() { Name = tagname };
                _dbContext.Tags.Add(tag);
            }
            post.Tags.Add(tag);
            return post;
        }

        private Post AddUser(string username, Post post)
        {
            var user = _dbContext.Users.Where(u => u.UserName == username).FirstOrDefault();
            if (user != null)
            {
                post.UserMentions.Add(user);
            }
            return post;
        }

        private void AddMentions(Post post, Func<string, Post, Post> getMention, char startsWith)
        {
            var mentions = post.Text.Split(' ')
                .Where(word => word.StartsWith(startsWith))
                .Select(word => word.Replace(startsWith.ToString(), ""))
                .Distinct()
                .ToList();
            
            foreach(var mention in mentions)
            {
                post = getMention(mention, post);
            }

            _dbContext.Posts.Update(post);
            _dbContext.SaveChanges();
        }

        public void AddTags(Post post)
        {
            AddMentions(post, AddTag, '#');
        }

        public void AddUsers(Post post)
        {
            AddMentions(post, AddUser, '@');
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
