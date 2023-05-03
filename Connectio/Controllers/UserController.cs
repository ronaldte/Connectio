using Connectio.Data;
using Connectio.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Connectio.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index(string username)
        {
            var user = _userRepository.GetUserByUserName(username);
            if(user == null)
            {
                return NotFound();
            }
            
            var viewModel = new ReadUserViewModel(user);
            return View(viewModel);
        }
    }
}
