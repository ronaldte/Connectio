using Connectio.Models;
using Connectio.Utilities;
using System.Text.RegularExpressions;

namespace Connectio.ViewModels
{
    /// <summary>
    /// ReadPost model represents the post entity for displaying purposes.
    /// </summary>
    public class ReadPostViewModel
    {
        /// <summary>
        /// Id of the post.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Text body content of the post.
        /// </summary>
        public string Text { get; set; } = string.Empty;
        
        /// <summary>
        /// Text formatted date and time of then was the post created.
        /// </summary>
        public string PostCreated { get; set; }
        
        /// <summary>
        /// UTC date and time of when was the activity created.
        /// </summary>
        public DateTime ActivityCreated { get; set; }
        
        /// <summary>
        /// User who created the post.
        /// </summary>
        public ReadUserViewModel User { get; set; }
        
        /// <summary>
        /// Title of the post used on activity.
        /// </summary>
        public string? Header { get; set; } = null;
        
        /// <summary>
        /// If post is of comment activity then Comment contains the comment entity which was created to the post.
        /// </summary>
        public ReadCommentViewModel? Comment { get; set; } = null;
        
        /// <summary>
        /// Type of activity made on the post. e.g. Like, Comment or new post.
        /// </summary>
        public ActivityType ActivityType { get; set; }
        
        /// <summary>
        /// User who created the reaction to the post.
        /// </summary>
        public string? ActivityUserName { get; set; }

        /// <summary>
        /// List of all comments on the post.
        /// </summary>
        public IEnumerable<ReadCommentViewModel>? Comments { get; set; }
        
        /// <summary>
        /// List of images on the post.
        /// </summary>
        public IEnumerable<ReadPostImageViewModel> Images { get; set; }

        public ReadPostViewModel(Post post)
        {
            Id = post.Id;
            Text = HightlightMentions(post.Text);
            PostCreated = post.Created.TimeSinceCreated();
            User = new ReadUserViewModel(post.User);
            Images = post.PostImages.Select(i => new ReadPostImageViewModel(i));
        }

        /// <summary>
        /// Method used for highlighting @... and #... mentions on the post.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private static string HightlightMentions(string text)
        {
            string pattern = @"\B([#@][a-zA-Z0-9(_)]+\b)";
            string replace = """<strong class="text-orange-400">$&</strong>""";
            return Regex.Replace(text, pattern, replace);
        }
    }
}
