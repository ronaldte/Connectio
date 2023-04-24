using Connectio.Models;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var posts = new List<PostModel>
            {
                new PostModel(){Id=1, Text="Hi!"},
                new PostModel(){Id=2, Text="How are you?"},
                new PostModel(){Id=3, Text="Great!"}
            };
            return View(posts);
        }
    }
}
