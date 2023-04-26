using Connectio.Models;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var posts = new List<Post>
            {
                new Post(){Id=1, Text="Hi!"},
                new Post(){Id=2, Text="How are you?"},
                new Post(){Id=3, Text="Great!"}
            };
            return View(posts);
        }
    }
}
