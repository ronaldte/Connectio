using Connectio.Data;
using Connectio.Models;
using Connectio.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.ViewComponents
{
    /// <summary>
    /// ViewComponent represents if logged in user follows another user with username.
    /// </summary>
    public class FollowViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;

        public FollowViewComponent(UserManager<ApplicationUser> userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Display button which states if user is following and can unfollow or user is not following and can follow.
        /// </summary>
        /// <param name="followingUsername">Username of user to follow or unfollow.</param>
        /// <returns>View which enables user to follow or unfollow other users.</returns>
        /// <exception cref="UnauthorizedAccessException">User must be logged in to follow other users.</exception>
        public async Task<IViewComponentResult> InvokeAsync(string followingUsername)
        {
            ApplicationUser following = _userRepository.GetUserByUserName(followingUsername)!;
            ApplicationUser follower = await _userManager.GetUserAsync(UserClaimsPrincipal) ?? throw new UnauthorizedAccessException();

            var followViewModel = new DisplayFollowViewModel()
            {
                FollowingName = followingUsername,
            };

            if (following.Followers.Contains(follower))
            {
                followViewModel.IsFollower = true;
            }
            else
            {
                followViewModel.IsFollower = false;
            }
            return View(followViewModel);
        }
    }
}
