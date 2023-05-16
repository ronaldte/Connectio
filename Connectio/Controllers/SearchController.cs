using Connectio.Data;
using Connectio.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.Controllers
{
    public class SearchController : Controller
    {
        private readonly ISearchRepository _searchRepository;

        public SearchController(ISearchRepository searchRepository)
        {
            _searchRepository = searchRepository;
        }

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

        public IActionResult Tag(string searchKeyword)
        {
            var tags = _searchRepository.GetTags(searchKeyword);

            var tagsViewModel = tags.Select(t => new ReadTagViewModel(t));

            var viewModel = new SearchTagViewModel(searchKeyword, tagsViewModel);
            return View(viewModel);
        }

        public IActionResult UserProfile(string searchKeyword)
        {
            var users = _searchRepository.GetUsers(searchKeyword);

            var usersViewModel = users.Select(u => new ReadUserViewModel(u));

            var viewModel = new SearchUserViewModel(searchKeyword, usersViewModel);
            return View(viewModel);
        }

        public IActionResult Post(string searchKeyword)
        {
            var posts = _searchRepository.GetPosts(searchKeyword);

            var postsViewModel = posts.Select(p =>new ReadPostViewModel(p));

            var viewModel = new SearchPostViewModel(searchKeyword, postsViewModel);
            return View(viewModel);
        }
    }
}
