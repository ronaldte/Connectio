using Connectio.Data;
using Connectio.Models;
using Connectio.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly IUserRepository _userRepository;
        private readonly IReactionRepository _reactionRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IPostRepository postRepository, UserManager<ApplicationUser> userManager, IUserRepository userRepository, IReactionRepository reactionRepository)
        {
            _postRepository = postRepository;
            _userManager = userManager;
            _userRepository = userRepository;
            _reactionRepository = reactionRepository;
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var following = _userRepository.GetUserByUserName(user.UserName!) ?? throw new UnauthorizedAccessException();

            var posts = new List<ReadPostViewModel>();
            foreach(var followee in following.Following)
            {
                var followeePosts = _postRepository.GetAllPostsByUser(followee.UserName!);
                foreach (var post in followeePosts)
                {
                    posts.Add(new ReadPostViewModel(post) {ActivityCreated=post.Created, ActivityUserName=followee.UserName});
                }

                var followeeLikes = _reactionRepository.GetAllUserLikes(followee);
                var header = $"{followee.DisplayName} (@{followee.UserName}) liked";
                foreach(var like in followeeLikes)
                {
                    posts.Add(new ReadPostViewModel(like.Post) { Header = header, ActivityCreated=like.Created, ActivityType = ActivityType.Like, ActivityUserName= followee.UserName });
                }

                var followeeComments = _reactionRepository.GetAllUserComments(followee);
                header = $"{followee.DisplayName}(@{followee.UserName}) commented on";
                foreach (var comment in followeeComments)
                {
                    posts.Add(new ReadPostViewModel(comment.Post) { Header = header, ActivityCreated=comment.Created, Comment=new ReadCommentViewModel(comment), ActivityType=ActivityType.Comment, ActivityUserName=followee.UserName});
                }
            }

            return View(posts.OrderByDescending(p => p.ActivityCreated).ToList());
        }
    }
}
