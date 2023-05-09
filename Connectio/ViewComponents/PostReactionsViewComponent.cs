using Connectio.Data;
using Connectio.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.ViewComponents
{
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


        public async Task<IViewComponentResult> InvokeAsync(int postId)
        {
            var post = _postRepository.GetPostById(postId)!;
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);

            if (user == null)
            {
                return View(new PostReactions(post));
            }

            var reactions = _reactionRepository.GetReactions(user, post);

            return View(reactions);
        }
    }
}
