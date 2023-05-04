using Connectio.Data;
using Connectio.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;

        public UserController(IUserRepository userRepository, IPostRepository postRepository)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
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
    }
}
