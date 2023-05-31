using Connectio.Data;
using Connectio.Models;
using Connectio.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.ViewComponents
{
    /// <summary>
    /// PostReactions represents bar for user reactions on given post.
    /// </summary>
    public class PostReactionsViewComponent : ViewComponent
    {
        private readonly IReactionRepository _reactionRepository;
        private readonly IPostRepository _postRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostReactionsViewComponent(IReactionRepository reactionRepository, IPostRepository postRepository, UserManager<ApplicationUser> userManager)
        {
            _reactionRepository = reactionRepository;
            _postRepository = postRepository;
            _userManager = userManager;
        }


        /// <summary>
        /// Displays status of reactions on given post.
        /// </summary>
        /// <param name="postId">Post for which reactions are gathered.</param>
        /// <returns>View in which user can react with the post.</returns>
        public async Task<IViewComponentResult> InvokeAsync(int postId)
        {
            var post = _postRepository.GetPostById(postId)!;
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);

            var reactionViewModel = new ReadReactionViewModel() {
                PostId = post.Id,
                CommentsCount = post.Comments.Count,
                LikesCount = post.Likes.Count,
                BookmarksCount = post.Bookmarks.Count,
            };

            // If user is logged in display if they liked or bookmarked the post
            if (user != null)
            {
                reactionViewModel.Liked = post.LikedBy.Any(l => l.UserName == user.UserName);
                reactionViewModel.Bookmarked = post.BookmarkedBy.Any(b => b.UserName == user.UserName);
            }

            return View(reactionViewModel);
        }
    }
}
