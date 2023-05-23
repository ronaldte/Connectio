using Connectio.Models;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.Controllers
{
    public class ConversationController : Controller
    {       
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(string username)
        {
            return View();
        }
        
        public IActionResult List()
        {
            return View();
        }
    }
}
