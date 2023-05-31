using Connectio.Data;
using Connectio.Models;
using Connectio.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.Controllers
{
    /// <summary>
    /// Reaction controller manages reactions on posts.
    /// </summary>
    [Authorize]
    public class ReactionController : Controller
    {
        private readonly IReactionRepository _reactionRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMentionRepository _mentionRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReactionController(IReactionRepository reactionRepository, IPostRepository postRepository, IMentionRepository mentionRepository, UserManager<ApplicationUser> userManager)
        {
            _reactionRepository = reactionRepository;
            _postRepository = postRepository;
            _mentionRepository = mentionRepository;
            _userManager = userManager;
        }

        /// <summary>
        /// Displays all likes on post.
        /// </summary>
        /// <param name="postId">Post id to view likes.</param>
        /// <returns>View containing all users, who liked the post.</returns>
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

        /// <summary>
        /// Displays logged in user bookmarks.
        /// </summary>
        /// <returns>View with list of all posts bookmarked by logged in user.</returns>
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

        /// <summary>
        /// Add post to user bookmarks.
        /// </summary>
        /// <param name="postId">Post id to be bookmarked.</param>
        /// <returns>View with all users' bookmarks.</returns>
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

        /// <summary>
        /// Remove post from user bookmarks.
        /// </summary>
        /// <param name="postId">Post id to be removed from bookmarks.</param>
        /// <returns>View with all users' bookmarks.</returns>
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

        /// <summary>
        /// Like a post.
        /// </summary>
        /// <param name="postId">Post to be liked.</param>
        /// <returns>Returns to home screen.</returns>
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

        /// <summary>
        /// Remove like from post.
        /// </summary>
        /// <param name="postId">Post from which like should be removed.</param>
        /// <returns>Returns to home screen.</returns>
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

        /// <summary>
        /// Displays form for writing new comment.
        /// </summary>
        /// <param name="postId">Post id to be commented.</param>
        /// <returns>View with form for writing new comment.</returns>
        public IActionResult CreateComment(int postId)
        {
            var post = _postRepository.GetPostById(postId);
            if(post == null)
            {
                return NotFound();
            }

            return View(new DisplayCreateCommentViewModel(post));
        }

        /// <summary>
        /// Creates new comment on a post.
        /// </summary>
        /// <param name="postId">Post id to be commented.</param>
        /// <param name="newComment">ViewModel for comment to be added.</param>
        /// <returns>View displaying post with newly added comment.</returns>
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

        /// <summary>
        /// Displays all posts with given tag.
        /// </summary>
        /// <param name="tagName">Tag to be viewed.</param>
        /// <returns>View containing all posts with given tag.</returns>
        [AllowAnonymous]
        public IActionResult PostsWithTag(string tagName)
        {
            var tag = _mentionRepository.GetTag(tagName);
            if (tag == null)
            {
                return NotFound();
            }

            var posts = _mentionRepository.GetPosts(tag);

            var tagViewModel = new ReadTagViewModel(tag);
            var postsViewModel = posts.Select(p => new ReadPostViewModel(p));
            
            var viewModel = new ReadPostsWithTagViewModel(tagViewModel, postsViewModel);
            return View(viewModel);
        }
    }
}
