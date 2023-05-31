namespace Connectio.Models
{
    /// <summary>
    /// Represents thread of text messages between users; chat room.
    /// </summary>
    public class Conversation
    {
        /// <summary>
        /// Key for Conversation.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// If true conversation is only between two users; otherwise multiple people can participate in chat.
        /// </summary>
        public bool IsPrivate { get; set; } = true;
        
        /// <summary>
        /// ApplicationUser who are allowed to read and write into the conversation.
        /// </summary>
        public List<ApplicationUser> Participants { get; set; } = new();
        
        /// <summary>
        /// Message history in conversation.
        /// </summary>
        public List<Message> Messages { get; set; } = new();
    }
}
