using Connectio.Data;
using Connectio.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.Controllers
{
    /// <summary>
    /// Controller that manages search results on social media.
    /// </summary>
    public class SearchController : Controller
    {
        private readonly ISearchRepository _searchRepository;

        public SearchController(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

        /// <summary>
        /// Displays first 5 search hits for posts, users and tags.
        /// </summary>
        /// <param name="searchKeyword">Search keyword to be found on social media.</param>
        /// <returns>View with up to 5 found items for posts, users and tags.</returns>
        public IActionResult Index(string searchKeyword)
        {
            var posts = _searchRepository.GetPosts(searchKeyword);
            var users = _searchRepository.GetUsers(searchKeyword);
            var tags = _searchRepository.GetTags(searchKeyword);

            var postsViewModel = posts.Take(5).Select(p => new ReadPostViewModel(p));
            var usersViewModel = users.Take(5).Select(u => new ReadUserViewModel(u));
            var tagsViewModel = tags.Take(5).Select(t => new ReadTagViewModel(t));

            var viewModel = new SearchViewModel(searchKeyword, postsViewModel, usersViewModel, tagsViewModel, posts.Count(), users.Count(), tags.Count());
            return View(viewModel);
        }

        /// <summary>
        /// Displays all tags that match searched keyword.
        /// </summary>
        /// <param name="searchKeyword">Seach keyword to be found in tags.</param>
        /// <returns>View containing all tags matching the searched keyword.</returns>
        public IActionResult Tag(string searchKeyword)
        {
            var tags = _searchRepository.GetTags(searchKeyword);

            var tagsViewModel = tags.Select(t => new ReadTagViewModel(t));

            var viewModel = new SearchTagViewModel(searchKeyword, tagsViewModel);
            return View(viewModel);
        }

        /// <summary>
        /// Displays all users that match searched keyword.
        /// </summary>
        /// <param name="searchKeyword">Search keyword to be found in users.</param>
        /// <returns>View containing all user matching the serached keyword.</returns>
        public IActionResult UserProfile(string searchKeyword)
        {
            var users = _searchRepository.GetUsers(searchKeyword);

            var usersViewModel = users.Select(u => new ReadUserViewModel(u));

            var viewModel = new SearchUserViewModel(searchKeyword, usersViewModel);
            return View(viewModel);
        }

        /// <summary>
        /// Displays all posts that match searched keyword.
        /// </summary>
        /// <param name="searchKeyword">Search keyword to be found in posts.</param>
        /// <returns>View containing all posts matching the searched keyword.</returns>
        public IActionResult Post(string searchKeyword)
        {
            var posts = _searchRepository.GetPosts(searchKeyword);

            var postsViewModel = posts.Select(p =>new ReadPostViewModel(p));

            var viewModel = new SearchPostViewModel(searchKeyword, postsViewModel);
            return View(viewModel);
        }
    }
}
