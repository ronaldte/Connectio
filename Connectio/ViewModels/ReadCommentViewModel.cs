using Connectio.Models;
using Connectio.Utilities;

namespace Connectio.ViewModels
{
    /// <summary>
    /// ReadComment model represents comment entity.
    /// </summary>
    public class ReadCommentViewModel
    {
        /// <summary>
        /// User who created the comment.
        /// </summary>
        public ReadUserViewModel User { get; set; }
        
        /// <summary>
        /// Body text content of the comment.
        /// </summary>
        public string Text { get; set; }
        
        /// <summary>
        /// String formated UTC date and timeto display when the post was created.
        /// </summary>
        public string Created { get; set; }

        public ReadCommentViewModel(Comment comment)
        {
            Text = comment.Text;
            Created = comment.Created.TimeSinceCreated();
            User = new ReadUserViewModel(comment.User);
        }
    }


}
