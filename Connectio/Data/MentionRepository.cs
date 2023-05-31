using Connectio.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Connectio.Data
{
    /// <inheritdoc/>
    public class MentionRepository : IMentionRepository
    {
        
        private readonly ConnectioDbContext _dbContext;

        public MentionRepository(ConnectioDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Adds tag mention to the post.
        /// </summary>
        /// <param name="tagname">Tag to be added to the post.</param>
        /// <param name="post">Post where tag is mentioned.</param>
        /// <returns>Post with tag added in mentions.</returns>
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

        /// <summary>
        /// Adds user mention to the post.
        /// </summary>
        /// <param name="username">User to be added to mentions in post.</param>
        /// <param name="post">Post where user is mentioned.</param>
        /// <returns>Post with user added in mentions.</returns>
        private Post AddUser(string username, Post post)
        {
            var user = _dbContext.Users.Where(u => u.UserName == username).FirstOrDefault();
            if (user != null)
            {
                post.UserMentions.Add(user);
            }
            return post;
        }

        /// <summary>
        /// Parses mentions from post.
        /// </summary>
        /// <param name="post">Post to parse.</param>
        /// <param name="addMention">Method for adding mention to post.</param>
        /// <param name="startsWith">Start character for mention.</param>
        private void AddMentions(Post post, Func<string, Post, Post> addMention, char startsWith)
        {
            /// Matches all @... for users or #... for tags
            var pattern = @"\B[#@]([a-zA-Z0-9(_)]+\b)";
            var rgx = new Regex(pattern);
            var matches = rgx.Matches(post.Text);

            /// Get unique matches, which start with given character with removed @/# srart symbol
            var mentions = matches.Where(m => m.Groups[0].Value.StartsWith(startsWith)).Select(m => m.Groups[1].Value).Distinct();

            foreach (var mention in mentions)
            {
                post = addMention(mention, post);
            }

            _dbContext.Posts.Update(post);
            _dbContext.SaveChanges();
        }

        /// <inheritdoc/>
        public void AddTags(Post post)
        {
            AddMentions(post, AddTag, '#');
        }

        /// <inheritdoc/>
        public void AddUsers(Post post)
        {
            AddMentions(post, AddUser, '@');
        }

        /// <inheritdoc/>
        public Tag? GetTag(string tagName)
        {
            return _dbContext.Tags.Where(t => t.Name == tagName).FirstOrDefault();
        }

        /// <inheritdoc/>
        public IEnumerable<Post> GetPosts(Tag tag)
        {
            var tagDb = _dbContext.Tags.Where(t => t == tag).Include(t => t.Posts).ThenInclude(p => p.User).First();
            return tagDb.Posts;
        }
    }
}
