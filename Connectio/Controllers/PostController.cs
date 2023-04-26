using Connectio.Data;
using Connectio.Models;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;

        public PostController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public IActionResult Index(int id)
        {
            var post = _postRepository.GetPostById(id);
            return View(post);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Post post)
        {
            if(ModelState.IsValid)
            {
                _postRepository.CreatePost(post);
                return RedirectToAction("Index", new { id = post.Id });
            }
            return View(post);
            
        }
    }
}
