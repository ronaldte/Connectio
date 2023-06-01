using System.ComponentModel.DataAnnotations;

namespace Connectio.ViewModels
{
    /// <summary>
    /// CreateMessage model represents new single message entity used for communication.
    /// </summary>
    public class CreateMessageViewModel
    {
        /// <summary>
        /// Conversation to whihc the message will be sent.
        /// </summary>
        [Required]
        public int ConversationId { get; set; }
        
        /// <summary>
        /// Body text content of the message.
        /// </summary>
        [Required]
        public string MessageText { get; set; } = string.Empty;
        
    }
}
