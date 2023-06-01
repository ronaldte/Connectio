using Connectio.Models;

namespace Connectio.ViewModels
{
    /// <summary>
    /// DisplayCreateComment display post which is being commented.
    /// </summary>
    public class DisplayCreateCommentViewModel
    {
        /// <summary>
        /// Post to which the comment is replaying to.
        /// </summary>
        public ReadPostViewModel Post { get; set; }
        
        /// <summary>
        /// Body text content of the comment.
        /// </summary>
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
