using Connectio.Data;
using Connectio.Models;
using Connectio.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.Controllers
{
    /// <summary>
    /// Post controller creates and displays posts
    /// </summary>
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

        /// <summary>
        /// Displays post with given id.
        /// </summary>
        /// <param name="id">Id of post to display</param>
        /// <returns>View containing PostViewModel</returns>
        public IActionResult Index(int id)
        {
            var post = _postRepository.GetPostById(id);
            if(post == null)
            {
                return NotFound();
            }

            var postViewModel = new ReadPostViewModel(post)
            {
                Comments = post.Comments.OrderByDescending(c => c.Created).Select(c => new ReadCommentViewModel(c))
            };
            return View(postViewModel);
        }
        
        /// <summary>
        /// Displays view for new post.
        /// </summary>
        /// <returns></returns>
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Saves given images to wwwroot with GUID names and adds links them to the post.
        /// </summary>
        /// <param name="images">List of IFormFiles with images to be saved.</param>
        /// <param name="post">Post which contains these images.</param>
        /// <returns>Post model with added images.</returns>
        private async Task<Post> SaveImages(List<IFormFile> images, Post post)
        {
            for (int i = 0; i < images.Count; i++)
            {
                // Naive method of checking image file extension, generate GUID as image file name and create new path
                var fileExtension = Path.GetExtension(images[i].FileName).ToLowerInvariant();
                var trustedFileNameForFileStorage = Guid.NewGuid().ToString() + fileExtension;
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), @"wwwroot\pictures", trustedFileNameForFileStorage);

                // Store image file to generated path
                using (var stream = System.IO.File.Create(filePath))
                {
                    await images[i].CopyToAsync(stream);
                }

                // Add name of the newly created iamge file to the image
                post.PostImages.Add(new PostImage() { ImageUrl = trustedFileNameForFileStorage, Order = i });
            }
            return post;
        }

        /// <summary>
        /// Creates new post.
        /// </summary>
        /// <param name="post">ViewModel of post to be saved.</param>
        /// <returns>View with post.</returns>
        /// <exception cref="ArgumentNullException"></exception>
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
