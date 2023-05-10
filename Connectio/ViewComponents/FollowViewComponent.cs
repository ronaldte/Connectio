using Connectio.Data;
using Connectio.Models;
using Connectio.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.ViewComponents
{
    public class FollowViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IUserRepository _userRepository;

        public FollowViewComponent(UserManager<ApplicationUser> userManager, IUserRepository userRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
        }

        public async Task<IViewComponentResult> InvokeAsync(string followingUsername)
        {
            ApplicationUser following = _userRepository.GetUserByUserName(followingUsername)!;
            ApplicationUser follower = await _userManager.GetUserAsync(UserClaimsPrincipal);

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
