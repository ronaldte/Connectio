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
        private readonly IMentionRepository _mentionRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public PostController(IPostRepository postRepository, UserManager<ApplicationUser> userManager, IMentionRepository mentionRepository)
        {
            _postRepository = postRepository;
            _userManager = userManager;
            _mentionRepository = mentionRepository;
        }

        public IActionResult Index(int id)
        {
            var post = _postRepository.GetPostById(id);
            if(post == null)
            {
                return NotFound();
            }

            var postViewModel = new ReadPostViewModel(post)
            {
                Comments = post.Comments.Select(c => new ReadCommentViewModel(c)).OrderByDescending(c => c.Created)
            };
            return View(postViewModel);
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

            _mentionRepository.AddTags(newPost);
            _mentionRepository.AddUsers(newPost);

            return RedirectToAction("Index", new { id = newPost.Id });
        }
    }
}
