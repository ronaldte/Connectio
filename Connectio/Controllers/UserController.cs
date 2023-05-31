using Connectio.Data;
using Connectio.Models;
using Connectio.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.Controllers
{
    /// <summary>
    /// User contorller manages profile specific items.
    /// </summary>
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

        /// <summary>
        /// Displays detailed user profile.
        /// </summary>
        /// <param name="username">User to be displayed.</param>
        /// <returns>View with user details and their posts.</returns>
        [AllowAnonymous]
        public IActionResult Index(string username)
        {
            var user = _userRepository.GetUserByUserName(username);
            if (user == null)
            {
                return NotFound();
            }

            var posts = _postRepository
                .GetAllPostsByUser(username)
                .OrderByDescending(p => p.Created)
                .Select(p => new ReadPostViewModel(p));

            var viewModel = new ReadUserViewModel(user, posts);
            return View(viewModel);
        }

        /// <summary>
        /// Start following new person.
        /// </summary>
        /// <param name="followingUsername">Username of user to be followed.</param>
        /// <returns>View with following user profile.</returns>
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

        /// <summary>
        /// Stop following a person.
        /// </summary>
        /// <param name="followingUsername">username of user to stop following.</param>
        /// <returns>View with following user profile.</returns>
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

        /// <summary>
        /// Displays user followers.
        /// </summary>
        /// <param name="username">User followers to be displayed.</param>
        /// <returns>View with all user followers.</returns>
        public IActionResult Followers(string username)
        {
            var user = _userRepository.GetUserByUserName(username);
            if (user == null)
            {
                return NotFound();
            }

            return View(new DisplayFollowerFollowingViewModel(user, user.Followers));
        }

        /// <summary>
        /// Displays user followings.
        /// </summary>
        /// <param name="username">User followings to be displayed.</param>
        /// <returns>View with all user followings.</returns>
        public IActionResult Following(string username)
        {
            var user = _userRepository.GetUserByUserName(username);
            if (user == null)
            {
                return NotFound();
            }

            return View(new DisplayFollowerFollowingViewModel(user, user.Following));
        }

        /// <summary>
        /// Saves given images to wwwroot with GUID names and adds links them to the post.
        /// </summary>
        /// <param name="images">List of IFormFiles with images to be saved.</param>
        /// <param name="post">Post which contains these images.</param>
        /// <returns>Post model with added images.</returns>
        private async Task SaveFile(IFormFile file, Action<ApplicationUser, string?> saveFunction)
        {
            var user = await _userManager.GetUserAsync(User) ?? throw new UnauthorizedAccessException();

            // Naive method of checking image file extension, generate GUID as image file name and create new path
            var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var trustedFileNameForFileStorage = Guid.NewGuid().ToString() + fileExtension;
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\pictures", trustedFileNameForFileStorage);

            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            saveFunction(user, trustedFileNameForFileStorage);
        }

        /// <summary>
        /// Displays view for updating user profile picture.
        /// </summary>
        /// <returns>View where user can update or remove their profile picture</returns>
        public IActionResult UpdateProfilePicture()
        {
            return View();
        }

        /// <summary>
        /// Updates user profile picture.
        /// </summary>
        /// <param name="picture">New profile picture.</param>
        /// <returns>View where user can update or remove their profile picture.</returns>
        [HttpPost]
        public async Task<IActionResult> UpdateProfilePicture(CreateFileViewModel picture)
        {
            if(!ModelState.IsValid)
            {
                return View();
            }

            await SaveFile(picture.File, _userRepository.UpdateProfilePicture);

            return RedirectToAction("UpdateProfilePicture");
        }

        /// <summary>
        /// Displays view for updating of user banner picture.
        /// </summary>
        /// <returns>View where user can update or remove their banner picture.</returns>
        public IActionResult UpdateBannerPicture()
        {
            return View();
        }

        /// <summary>
        /// Updates user baner picture to new uploaded picture.
        /// </summary>
        /// <param name="picture">New picture.</param>
        /// <returns>View where user can update or remove their banner picture</returns>
        [HttpPost]
        public async Task<IActionResult> UpdateBannerPicture(CreateFileViewModel picture)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            await SaveFile(picture.File, _userRepository.UpdateBannerPicture);

            return RedirectToAction("UpdateBannerPicture");
        }
        
        /// <summary>
        /// Removes picture from profile setting it to default value.
        /// </summary>
        /// <param name="removeFunction">Function to use for removal of picture.</param>
        /// <exception cref="UnauthorizedAccessException">Picture can be removed only from logged in user.</exception>
        private async Task RemovePicture(Action<ApplicationUser, string?> removeFunction)
        {
            var user = await _userManager.GetUserAsync(User) ?? throw new UnauthorizedAccessException();
            removeFunction(user, null);
        }

        /// <summary>
        /// Removes profile picture returning it to default state.
        /// </summary>
        /// <returns>Returns to Update profile View</returns>
        [HttpPost]
        public async Task<IActionResult> RemoveProfilePicture()
        {
            await RemovePicture(_userRepository.UpdateProfilePicture);

            return RedirectToAction("UpdateProfilePicture");
        }

        /// <summary>
        /// Removes banner picture returning it to default state.
        /// </summary>
        /// <returns>Returns to Update banner View</returns>
        [HttpPost]
        public async Task<IActionResult> RemoveBannerPicture()
        {
            await RemovePicture(_userRepository.UpdateBannerPicture);

            return RedirectToAction("UpdateBannerPicture");
        }
    }
}
