﻿using Connectio.Data;
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
    }
}