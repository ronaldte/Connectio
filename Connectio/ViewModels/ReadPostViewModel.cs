using Connectio.Models;

namespace Connectio.ViewModels
{
    public class ReadPostViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
        public string Created { get; set; }
        public ApplicationUser User { get; set; }

        public ReadPostViewModel(Post post)
        {
            Id = post.Id;
            Text = post.Text;
            Created = CalculateAge(post.Created);
            User = post.User;
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
