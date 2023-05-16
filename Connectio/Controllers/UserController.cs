using Connectio.Data;
using Connectio.Models;
using Connectio.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IUserRepository userRepository, IPostRepository postRepository, UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult Index(string username)
        {
            var user = _userRepository.GetUserByUserName(username);
            if(user == null)
            {
                return NotFound();
            }

            List<ReadPostViewModel> posts = new();
            var postsFromDb = _postRepository.GetAllPostsByUser(username);
            foreach(var post in postsFromDb)
            {
                posts.Add(new ReadPostViewModel(post));
            }
            
            var viewModel = new ReadUserViewModel(user, posts);
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFollower(string followingUsername)
        {
            var following = _userRepository.GetUserByUserName(followingUsername);
            if (following == null)
            {
                return NotFound();
            }

            var follower = await _userManager.GetUserAsync(User);
            if (follower == null)
            {
                return NotFound();
            }

            if (follower == following)
            {
                return BadRequest();
            }

            following.Followers.Add(follower);
            _userRepository.UpdateFollower(following);

            return RedirectToAction("Index", "User", new { username = followingUsername});
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteFollower(string followingUsername)
        {
            var following = _userRepository.GetUserByUserName(followingUsername);
            if (following == null)
            {
                return NotFound();
            }

            var follower = await _userManager.GetUserAsync(User);
            if (follower == null)
            {
                return NotFound();
            }

            if (follower == following)
            {
                return BadRequest();
            }

            following.Followers.Remove(follower);
            _userRepository.UpdateFollower(following);

            return RedirectToAction("Index", "User", new { username = followingUsername});
        }

        public IActionResult Followers(string username)
        {
            var user = _userRepository.GetUserByUserName(username);
            if (user == null)
            {
                return NotFound();
            }

            return View(new DisplayFollowerFollowingViewModel(user, user.Followers));
        }

        public IActionResult Following(string username)
        {
            var user = _userRepository.GetUserByUserName(username);
            if (user == null)
            {
                return NotFound();
            }

            return View(new DisplayFollowerFollowingViewModel(user, user.Following));
        }
    }
}
