using Connectio.Data;
using Connectio.Hubs;
using Connectio.Models;
using Connectio.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Connectio.Controllers
{
    /// <summary>
    /// Conversation controller manages messages between two users.
    /// </summary>
    [Authorize]
    public class ConversationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IConversationRepository _conversationRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHubContext<MessageHub> _messageHub;

        public ConversationController(IUserRepository userRepository, IConversationRepository conversationRepository, UserManager<ApplicationUser> userManager, IHubContext<MessageHub> messageHub)
        {
            _userRepository = userRepository;
            _conversationRepository = conversationRepository;
            _userManager = userManager;
            _messageHub = messageHub;
        }
        
        /// <summary>
        /// Displays view for creating new conversation or conversation thread if such exists.
        /// </summary>
        /// <param name="username">Username of user to create new conversation with.</param>
        /// <returns>View for new conversation message or conversation thread if such exists.</returns>
        /// <exception cref="UnauthorizedAccessException">User should be logged in to message someone.</exception>
        public async Task<IActionResult> Create(string? username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return View();
            }

            var userFrom = await _userManager.GetUserAsync(User) ?? throw new UnauthorizedAccessException();

            var userToMessage = _userRepository.GetUserByUserName(username);
            if(userToMessage is not null)
            {
                var existingConversation = _conversationRepository.GetConversation(userFrom, userToMessage);
                if (existingConversation is not null)
                {
                    return RedirectToAction("Read", new { conversationId = existingConversation.Id });
                }
            }

            var conversation = new CreateConversationViewModel() { ToUser = username };
            if(userToMessage is null)
            {
                ModelState.AddModelError(nameof(conversation.ToUser), "This user does not exist");
            }

            return View(conversation);
        }

        /// <summary>
        /// Create new conversation with user with provided first message.
        /// </summary>
        /// <param name="conversation">Model representing new covnersation</param>
        /// <returns>Newly created conversation thread with user with provided first message.</returns>
        /// <exception cref="UnauthorizedAccessException">User should be logged in to create conversation with user.</exception>
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

            await NotifyConversation(newConversation.Id, userFrom, newMessage);

            return RedirectToAction("Read", new {conversationId = newConversation.Id});
        }
        
        /// <summary>
        /// Displays view with all conversation logged in user is participating in.
        /// </summary>
        /// <returns>View with list of all conversation view models</returns>
        /// <exception cref="UnauthorizedAccessException">User should be logged in to view conversations.</exception>
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

        /// <summary>
        /// Displays conversation thread with messages of given conversation.
        /// </summary>
        /// <param name="conversationId">ID of conversation to display.</param>
        /// <returns>View with list of last 10 messages.</returns>
        /// <exception cref="UnauthorizedAccessException">User should be logged in to read a conversation.</exception>
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

        /// <summary>
        /// Creates new message and posts it into a conversation thread.
        /// </summary>
        /// <param name="message">ViewModel for new message.</param>
        /// <returns>Redirection to conversation thread with newly created message.</returns>
        /// <exception cref="UnauthorizedAccessException">User should be logged in to send messages.</exception>
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

            await NotifyConversation(conversation.Id, user, newMessage);

            return RedirectToAction("Read", new { conversationId = message.ConversationId });
        }

        /// <summary>
        /// Sends message from user to all participants in conversation except user who sent it.
        /// </summary>
        /// <param name="conversationId">Conversation Id to notify participants of.</param>
        /// <param name="fromUser">User who created the notification.</param>
        /// <param name="message">Message which was created.</param>
        private async Task NotifyConversation(int conversationId, ApplicationUser fromUser, Message message)
        {
            var conversation = _conversationRepository.GetParticipants(conversationId)!.Where(u => u != fromUser);
            foreach (var user in conversation)
            {
                await _messageHub.Clients.User(user.Id).SendAsync("Notify", fromUser.UserName, message.Text, conversationId);
            }
        }
    }
}
