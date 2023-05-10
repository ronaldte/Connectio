using Connectio.Models;

namespace Connectio.ViewModels
{
    public class ReadPostCommentsViewModel
    {
        public ReadPostViewModel Post { get; set; }
        public List<ReadCommentViewModel> Comments { get; set; }

        public ReadPostCommentsViewModel(Post post, List<Comment> comments)
        {
            Post = new ReadPostViewModel(post);
            Comments = new List<ReadCommentViewModel>();
            foreach(var comment in comments)
            {
                Comments.Add(new ReadCommentViewModel(comment));
            }
        }
    }
}
