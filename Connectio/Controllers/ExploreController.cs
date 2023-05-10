﻿using Connectio.Data;
using Connectio.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.Controllers
{
    public class ExploreController : Controller
    {
        private readonly IPostRepository _postRepository;

        public ExploreController(IPostRepository postRepository)
        {
            _postRepository = postRepository;
        }

        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPosts().OrderByDescending(p => p.Created);
            var postsViewModel = new List<ReadPostViewModel>();

            foreach (var post in posts)
            {
                postsViewModel.Add(new ReadPostViewModel(post));
            }

            return View(postsViewModel);
        }
    }
}