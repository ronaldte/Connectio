using Connectio.Data;
using Connectio.Models;
using Connectio.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.Controllers
{
    [Authorize]
    public class ReactionController : Controller
    {
        private readonly IReactionRepository _reactionRepository;
        private readonly IPostRepository _postRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReactionController(IReactionRepository reactionRepository, IPostRepository postRepository, UserManager<ApplicationUser> userManager)
        {
            _reactionRepository = reactionRepository;
            _postRepository = postRepository;
            _userManager = userManager;
        }

        public IActionResult DisplayPostLikes(int postId)
        {
            var post = _postRepository.GetPostById(postId);
            if(post == null)
            {
                return NotFound();
            }

            var likedBy = new ReadPostLikesViewModel(post.LikedBy, post);
            
            return View(likedBy);
        }

        public async Task<IActionResult> UserBookmarks()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound();
            }

            var bookmarks = _reactionRepository.GetAllBookmarks(user);

            return View(new ReadAllBookmarksViewModel(bookmarks));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBookmark(int postId)
        {
            var post = _postRepository.GetPostById(postId);
            var user = await _userManager.GetUserAsync(User);
            if (user == null || post == null)
            {
                return NotFound();
            }

            var bookmark = new Bookmark()
            {
                User = user,
                Post = post
            };
            _reactionRepository.CreateBookmark(bookmark);
            return RedirectToAction("UserBookmarks");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBookmark(int postId)
        {
            var post = _postRepository.GetPostById(postId);
            var user = await _userManager.GetUserAsync(User);
            if (user == null || post == null)
            {
                return NotFound();
            }

            var bookmark = _reactionRepository.GetBookmark(user, post);
            if(bookmark == null)
            {
                return NotFound();
            }

            _reactionRepository.DeleteBookmark(bookmark);
            return RedirectToAction("UserBookmarks");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateLike(int postId)
        {
            var post = _postRepository.GetPostById(postId);
            var user = await _userManager.GetUserAsync(User);
            if (user == null || post == null)
            {
                return NotFound();
            }

            var like = new Like()
            {
                User = user,
                Post = post
            };
            _reactionRepository.CreateLike(like);
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteLike(int postId)
        {
            var post = _postRepository.GetPostById(postId);
            var user = await _userManager.GetUserAsync(User);
            if (user == null || post == null)
            {
                return NotFound();
            }

            var like = _reactionRepository.GetLike(user, post);
            if (like == null)
            {
                return NotFound();
            }

            _reactionRepository.DeleteLike(like);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult CreateComment(int postId)
        {
            var post = _postRepository.GetPostById(postId);
            if(post == null)
            {
                return NotFound();
            }

            return View(new DisplayCreateCommentViewModel(post));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment(int postId, CreateCommentViewModel newComment)
        {

            var post = _postRepository.GetPostById(postId);
            var user = await _userManager.GetUserAsync(User);
            if (user == null || post == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(new DisplayCreateCommentViewModel(post, newComment.Text));
            }
            
            var comment = new Comment()
            {
                User = user,
                Post = post,
                Text = newComment.Text
            };
            _reactionRepository.CreateComment(comment);
            return RedirectToAction("Index", "Post", new { id = postId });
        }
    }
}
