using Connectio.Models;

namespace Connectio.ViewModels
{
    /// <summary>
    /// ReadPostLikes model represents users who likes the post.
    /// </summary>
    public class ReadPostLikesViewModel
    {
        /// <summary>
        /// Post on which likes are being displayed.
        /// </summary>
        public ReadPostViewModel Post { get; set; }
        
        /// <summary>
        /// Users who liked the post.
        /// </summary>
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
