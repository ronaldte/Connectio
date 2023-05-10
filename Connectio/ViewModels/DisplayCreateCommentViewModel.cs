using Connectio.Models;

namespace Connectio.ViewModels
{
    public class DisplayCreateCommentViewModel
    {
        public ReadPostViewModel Post { get; set; }
        public string Text { get; set; } = string.Empty;

        public DisplayCreateCommentViewModel(Post post)
        {
            Post = new ReadPostViewModel(post);
        }

        public DisplayCreateCommentViewModel(Post post, string text) : this(post)
        {
            Text = text;
        }
    }
}
