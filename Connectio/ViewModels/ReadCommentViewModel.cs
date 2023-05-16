using Connectio.Models;
using Connectio.Utils;

namespace Connectio.ViewModels
{
    public class ReadCommentViewModel
    {
        public ReadUserViewModel User { get; set; }
        public string Text { get; set; }
        public string Created { get; set; }

        public ReadCommentViewModel(Comment comment)
        {
            Text = comment.Text;
            Created = comment.Created.TimeSinceCreated();
            User = new ReadUserViewModel(comment.User);
        }
    }


}
