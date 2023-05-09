using Connectio.Models;

namespace Connectio.ViewModels
{
    public class ReadPostLikesViewModel
    {
        public ReadPostViewModel Post { get; set; }
        public IEnumerable<ReadUserViewModel> LikedBy { get; set; }

        public ReadPostLikesViewModel(List<ApplicationUser> users, Post post)
        {
            var likedBy = new List<ReadUserViewModel>();
            foreach (var user in users)
            {
                likedBy.Add(new ReadUserViewModel(user));
            }
            LikedBy = likedBy;
            Post = new ReadPostViewModel(post);

        }
    }
}
