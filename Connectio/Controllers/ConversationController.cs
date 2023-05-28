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
        private readonly IConversationRepository _conversationRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ConversationController(IUserRepository userRepository, IConversationRepository conversationRepository, UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _conversationRepository = conversationRepository;
            _userManager = userManager;
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateConversationViewModel conversation)
        {
            if(!ModelState.IsValid)
            {
                return View(conversation);
            }

            var userToMessage = _userRepository.GetUserByUserName(conversation.ToUser);
            if(userToMessage is null)
            {
                ModelState.AddModelError(nameof(conversation.ToUser), "This user does not exist");
                return View(conversation);
            }

            var userFrom = await _userManager.GetUserAsync(User) ?? throw new UnauthorizedAccessException();

            var newConversation = new Conversation();
            newConversation.Participants.AddRange(new List<ApplicationUser> { userFrom, userToMessage });

            var newMessage = new Message() { Text = conversation.MessageText, CreatedBy = userFrom, Conversation = newConversation };

            _conversationRepository.CreateConversation(newConversation);
            _conversationRepository.CreateMessage(newMessage);
            _conversationRepository.SaveChanges();

            return RedirectToAction("List");
        }
        
        public async Task<IActionResult> List()
        {
            var user = await _userManager.GetUserAsync(User) ?? throw new UnauthorizedAccessException();
            var conversations = _conversationRepository.GetUserConversations(user);
            
            var viewModel = new List<DisplayConversationListViewModel>();
            foreach(var conversation in conversations)
            {
                var conversationViewModel = new DisplayConversationListViewModel()
                {
                    Id = conversation.Id,
                    IsPrivate = conversation.IsPrivate,
                    Participants = conversation.Participants.Where(p => p != user).Select(p => new ReadUserViewModel(p)).ToList(),
                    LastMessage = _conversationRepository.GetMessages(conversation.Id, 1)!.First()
                };

                viewModel.Add(conversationViewModel);
            }
            
            return View(viewModel);
        }

        public async Task<IActionResult> Read(int conversationId)
        {
            if (!_conversationRepository.ConversationExists(conversationId))
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User) ?? throw new UnauthorizedAccessException();

            var conversations = _conversationRepository.GetUserConversations(user);
            if(!conversations.Any(c => c.Id == conversationId))
            {
                return Unauthorized();
            }

            var messages = _conversationRepository.GetMessages(conversationId);

            var conversation = conversations.Where(c => c.Id == conversationId).First();
            var viewModel = new DisplayConversationViewModel()
            {
                Id = conversation.Id,
                IsPrivate = conversation.IsPrivate,
                Participants = conversation.Participants.Where(p => p != user).Select(p => new ReadUserViewModel(p)).ToList(),
                Messages = messages.Select(m => new ReadMessageViewModel(m)).ToList()
            };
            
            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Send(CreateMessageViewModel message)
        {
            // Prevent manually altering HTML Form in view.
            if (!ModelState.IsValid)
            {
                return RedirectToAction("List");
            }

            if (!_conversationRepository.ConversationExists(message.ConversationId))
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User) ?? throw new UnauthorizedAccessException();

            var conversations = _conversationRepository.GetUserConversations(user);
            if (!conversations.Any(c => c.Id == message.ConversationId))
            {
                return Unauthorized();
            }

            var conversation = conversations.Where(c => c.Id == message.ConversationId).First();
            var newMessage = new Message() { Text = message.MessageText, CreatedBy = user, Conversation=conversation};

            _conversationRepository.CreateMessage(newMessage);
            _conversationRepository.SaveChanges();

            return RedirectToAction("Read", new { conversationId = message.ConversationId });
        }
    }
}
