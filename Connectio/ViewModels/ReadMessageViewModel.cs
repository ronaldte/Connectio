using Connectio.Models;

namespace Connectio.ViewModels
{
    /// <summary>
    /// ReadMessage model represents message entity.
    /// </summary>
    public class ReadMessageViewModel
    {
        /// <summary>
        /// Id of the message.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// User who created the message.
        /// </summary>
        public ReadUserViewModel CreatedBy { get; set; } = null!;
        
        /// <summary>
        /// UTC date and time when was the message created.
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Body text content of the message.
        /// </summary>
        public string Text { get; set; } = null!;
        
        /// <summary>
        /// Conversation Id to which the message was sent.
        /// </summary>
        public int ConversationId { get; set; }

        public ReadMessageViewModel(Message message)
        {
            Id = message.Id;
            CreatedBy = new ReadUserViewModel(message.CreatedBy);
            CreatedAt = message.CreatedAt;
            Text = message.Text;
            ConversationId = message.Conversation.Id;
        }
    }
}
