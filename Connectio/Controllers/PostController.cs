using Connectio.Data;
using Connectio.Models;
using Connectio.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostController(IPostRepository postRepository, UserManager<ApplicationUser> userManager)
        {
            _postRepository = postRepository;
            _userManager = userManager;
        }

        public IActionResult Index(int id)
        {
            var post = _postRepository.GetPostById(id);
            return View(post);
        }
        
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create(CreatePostViewModel post)
        {
            if (!ModelState.IsValid)
            {
                return View(post);
            }

            var newPost = new Post()
            {
                Created = DateTime.UtcNow,
                User = await _userManager.GetUserAsync(User) ?? throw new ArgumentNullException(nameof(User)),
                Text = post.Text
            };
                
            _postRepository.CreatePost(newPost);
            return RedirectToAction("Index", new { id = newPost.Id });
        }
    }
}
