using Connectio.Data;
using Connectio.Models;
using Connectio.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.Controllers
{
    [Authorize]
    public class ConversationController : Controller
    {
        private readonly IUserRepository _userRepository;

        public ConversationController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateConversationViewModel newConversation)
        {
            if(!ModelState.IsValid)
            {
                return View(newConversation);
            }

            var userToMessage = _userRepository.GetUserByUserName(newConversation.ToUser);
            if(userToMessage is null)
            {
                ModelState.AddModelError(nameof(newConversation.ToUser), "This user does not exist");
                return View(newConversation);
            }

            return View();
        }
        
        public IActionResult List()
        {
            return View();
        }
    }
}
