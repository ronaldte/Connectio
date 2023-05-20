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

        private async Task<Post> SaveImages(List<IFormFile> images, Post post)
        {
            for (int i = 0; i < images.Count; i++)
            {
                var fileExtension = Path.GetExtension(images[i].FileName).ToLowerInvariant();
                var trustedFileNameForFileStorage = Guid.NewGuid().ToString() + fileExtension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\pictures", trustedFileNameForFileStorage);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await images[i].CopyToAsync(stream);
                }

                post.PostImages.Add(new PostImage() { ImageUrl = trustedFileNameForFileStorage, Order = i });
            }
            return post;
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

            var images = new List<IFormFile>() { post.ImageFile1, post.ImageFile2, post.ImageFile3 }
                .Where(img => img != null)
                .ToList();

            newPost = await SaveImages(images, newPost);
            
            _postRepository.CreatePost(newPost);

            _mentionRepository.AddTags(newPost);
            _mentionRepository.AddUsers(newPost);

            return RedirectToAction("Index", new { id = newPost.Id });
        }
    }
}
