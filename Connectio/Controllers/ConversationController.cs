﻿using Connectio.Data;
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
    }
}
