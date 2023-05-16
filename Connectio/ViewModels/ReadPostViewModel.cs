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
        public ApplicationUser User { get; set; }
        public string? Header { get; set; } = null;
        public Comment? Comment { get; set; } = null;
        public ActivityType ActivityType { get; set; }
        public string? ActivityUserName { get; set; }
        public IEnumerable<ReadCommentViewModel>? Comments { get; set; }

        public ReadPostViewModel(Post post)
        {
            Id = post.Id;
            Text = HightlightMentions(post.Text);
            PostCreated = post.Created.TimeSinceCreated();
            User = post.User;
        }

        private static string HightlightMentions(string text)
        {
            string pattern = @"\B([#@][a-zA-Z0-9(_)]+\b)";
            string replace = """<strong class="text-orange-400">$&</strong>""";
            return Regex.Replace(text, pattern, replace);
        }

        private static string CalculateAge(DateTime created)
        {
            var timeDifference = DateTime.UtcNow - created;

            string response;

            if (timeDifference.TotalDays > 365)
            {
                response = created.ToString("MMM yyyy");
            }
            else if (timeDifference.TotalHours > 24)
            {
                response = created.ToString("d MMM");
            }
            else if(timeDifference.TotalHours > 1)
            {
                response = $"{timeDifference.Hours}h";
            }
            else if( timeDifference.TotalMinutes > 1)
            {
                response = $"{timeDifference.Minutes}m";
            }
            else
            {
                response = "Just now";
            }

            return response;
        }
    }
}
