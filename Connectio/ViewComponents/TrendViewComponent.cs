using Connectio.Data;
using Connectio.Models;
using Connectio.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.ViewComponents
{
    /// <summary>
    /// Trend represents few people user can follow and trending tags to check out.
    /// </summary>
    public class TrendViewComponent : ViewComponent
    {
        private readonly ITrendRepository _trendRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public TrendViewComponent(ITrendRepository trendRepository, UserManager<ApplicationUser> userManager)
        {
            _trendRepository = trendRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// Displays users to follow and tags to checkout in which or whom logged in user may be interested.
        /// </summary>
        /// <returns>View containing other users and tags.</returns>
        /// <exception cref="UnauthorizedAccessException">User must be logged in to see trends.</exception>
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal) ?? throw new UnauthorizedAccessException();

            var peopleToFollow = _trendRepository.GetPeopleToFollow(user).Select(u => new ReadUserViewModel(u));
            var trendingTags = _trendRepository.GetTrendingTags().Select(t => new ReadTagViewModel(t));

            var viewModel = new ReadTrendViewModel(peopleToFollow, trendingTags);
            return View(viewModel);
        }
    }
}
