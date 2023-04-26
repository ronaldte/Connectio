using Connectio.Data;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPostRepository _postRepository;

        public HomeController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPosts();
            return View(posts);
        }
    }
}
