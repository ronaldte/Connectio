using Connectio.ViewModels;
using Connectio.ViewModels.Post;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var posts = new List<PostReadViewModel>
            {
                new PostReadViewModel(1, "Hello, world"),
                new PostReadViewModel(2, "How are you doint?"),
                new PostReadViewModel(3, "Great!")
            };
            return View(new HomeViewModel(posts));
        }
    }
}
