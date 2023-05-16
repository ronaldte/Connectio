using Connectio.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

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
            var pattern = @"\B[#@]([a-zA-Z0-9(_)]+\b)";
            var rgx = new Regex(pattern);
            var matches = rgx.Matches(post.Text);

            var mentions = matches.Select(m => m.Groups[1].Value).Distinct();

            foreach (var mention in mentions)
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

        public Tag? GetTag(string tagName)
        {
            return _dbContext.Tags.Where(t => t.Name == tagName).FirstOrDefault();
        }

        public IEnumerable<Post> GetPosts(Tag tag)
        {
            var tagDb = _dbContext.Tags.Where(t => t == tag).Include(t => t.Posts).ThenInclude(p => p.User).First();
            return tagDb.Posts;
        }
    }
}
