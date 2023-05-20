using Connectio.Models;
using Connectio.Utils;
using System.Text.RegularExpressions;

namespace Connectio.ViewModels
{
    public class ReadPostViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public string PostCreated { get; set; }
        public DateTime ActivityCreated { get; set; }
        public ReadUserViewModel User { get; set; }
        public string? Header { get; set; } = null;
        public ReadCommentViewModel? Comment { get; set; } = null;
        public ActivityType ActivityType { get; set; }
        public string? ActivityUserName { get; set; }
        public IEnumerable<ReadCommentViewModel>? Comments { get; set; }
        public IEnumerable<ReadPostImageViewModel> Images { get; set; }

        public ReadPostViewModel(Post post)
        {
            Id = post.Id;
            Text = HightlightMentions(post.Text);
            PostCreated = post.Created.TimeSinceCreated();
            User = new ReadUserViewModel(post.User);
            Images = post.PostImages.Select(i => new ReadPostImageViewModel(i));
        }

        private static string HightlightMentions(string text)
        {
            string pattern = @"\B([#@][a-zA-Z0-9(_)]+\b)";
            string replace = """<strong class="text-orange-400">$&</strong>""";
            return Regex.Replace(text, pattern, replace);
        }
    }
}
