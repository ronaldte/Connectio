namespace Connectio.Models
{
    /// <summary>
    /// Represents short though in form of text which is added to conversation for others to see and react to.
    /// </summary>
    public class Message
    {
        /// <summary>
        /// Key of Message.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Who created and then added the message.
        /// </summary>
        public ApplicationUser CreatedBy { get; set; } = null!;
        
        /// <summary>
        /// UTC date and time when the message was sent.
        /// </summary>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// Body text content of the message.
        /// </summary>
        public string Text { get; set; } = null!;
        
        /// <summary>
        /// Conversation in which was the message sent to.
        /// </summary>
        public Conversation Conversation { get; set; } = null!;
    }
}
