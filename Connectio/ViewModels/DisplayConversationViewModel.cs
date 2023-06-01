namespace Connectio.ViewModels
{
    /// <summary>
    /// DisplayConversation model display conversation itself by showing messages which are in the conversation.
    /// </summary>
    public class DisplayConversationViewModel
    {
        
        /// <summary>
        /// Id of the conversation.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// If true conversation is only between two participants; otherwise multiple people can chat.
        /// </summary>
        public bool IsPrivate { get; set; }
        
        /// <summary>
        /// List of participating users in the conversation.
        /// </summary>
        public List<ReadUserViewModel> Participants { get; set; } = new();
        
        /// <summary>
        /// List of messages added to the conversation.
        /// </summary>
        public List<ReadMessageViewModel> Messages { get; set; } = new();
    }
}
