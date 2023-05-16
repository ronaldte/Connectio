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
            var posts = _searchRepository.GetPosts(searchKeyword).Take(5);
            var users = _searchRepository.GetUsers(searchKeyword).Take(5);
            var tags = _searchRepository.GetTags(searchKeyword).Take(5);

            var postsViewModel = posts.Select(p => new ReadPostViewModel(p));
            var usersViewModel = users.Select(u => new ReadUserViewModel(u));
            var tagsViewModel = tags.Select(t => new ReadTagViewModel(t));

            var viewModel = new SearchViewModel(searchKeyword, postsViewModel, usersViewModel, tagsViewModel);
            return View(viewModel);
        }

        public IActionResult Tag(string searchString)
        {
            var tags = _searchRepository.GetTags(searchString);

            var tagsViewModel = tags.Select(t => new ReadTagViewModel(t));
            return View(tagsViewModel);
        }

        public IActionResult UserProfile(string searchString)
        {
            var users = _searchRepository.GetUsers(searchString);

            var usersViewModel = users.Select(u => new ReadUserViewModel(u));
            return View(usersViewModel);
        }

        public IActionResult Post(string searchString)
        {
            var posts = _searchRepository.GetPosts(searchString);

            var postsViewModel = posts.Select(p =>new ReadPostViewModel(p));
            return View(postsViewModel);
        }
    }
}
