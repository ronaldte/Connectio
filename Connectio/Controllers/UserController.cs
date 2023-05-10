using Connectio.Data;
using Connectio.Models;
using Connectio.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.Controllers
{
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

            following.Followers.Add(follower);
            _userRepository.UpdateFollower(following);

            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
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

            following.Followers.Remove(follower);
            _userRepository.UpdateFollower(following);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult GetFollowers(string username)
        {
            var user = _userRepository.GetUserByUserName(username);
            if (user == null)
            {
                return NotFound();
            }

            return View(new DisplayFollowerFollowingViewModel(user, user.Followers));
        }

        public IActionResult GetFollowings(string username)
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
