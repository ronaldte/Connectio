using Connectio.Models;

namespace Connectio.ViewModels
{
    /// <summary>
    /// DisplayConversation is model representing short description of a conversation.
    /// </summary>
    public class DisplayConversationListViewModel
    {
        /// <summary>
        /// Id of conversation.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// If true, conversation is between two users; otherwise multiple people are participating.
        /// </summary>
        public bool IsPrivate { get; set; }
        
        /// <summary>
        /// List of participants in the conversation.
        /// </summary>
        public List<ReadUserViewModel> Participants { get; set; } = new();
        
        /// <summary>
        /// Last message, which was added to the conversation.
        /// </summary>
        public Message LastMessage { get; set; } = null!;
    }
}
