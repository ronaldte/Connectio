using Connectio.Models;

namespace Connectio.ViewModels
{
    public class ReadCommentViewModel
    {
        public ReadUserViewModel User { get; set; }
        public string Text { get; set; }
        public DateTime Created { get; set; }

        public ReadCommentViewModel(Comment comment)
        {
            Text = comment.Text;
            Created = comment.Created;
            User = new ReadUserViewModel(comment.User);
        }
    }


}
