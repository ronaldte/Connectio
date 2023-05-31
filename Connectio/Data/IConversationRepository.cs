using Connectio.Models;

namespace Connectio.Data
{
    /// <summary>
    /// Conversations manages chat between users.
    /// </summary>
    public interface IConversationRepository
    {
        /// <summary>
        /// Saves changes to DB.
        /// </summary>
        void SaveChanges();
        
        /// <summary>
        /// Add new conversation to DB.
        /// </summary>
        /// <param name="conversation">Conversation model to be added to DB.</param>
        void CreateConversation(Conversation conversation);
        
        /// <summary>
        /// Add new message to DB.
        /// </summary>
        /// <param name="message">Message model to be added to DB.</param>
        void CreateMessage(Message message);
        
        /// <summary>
        /// Get all conversation where user is a part of.
        /// </summary>
        /// <param name="user">User for which conversations will be loaded.</param>
        /// <returns>List of all conversations user participates in.</returns>
        IEnumerable<Conversation> GetUserConversations(ApplicationUser user);
        
        /// <summary>
        /// Get signle conversation between two users.
        /// </summary>
        /// <param name="firstUser">First person in conversation.</param>
        /// <param name="secondUser">Second person in conversation.</param>
        /// <returns>Conversation between given two users if exists, null otherwise.</returns>
        Conversation? GetConversation(ApplicationUser firstUser, ApplicationUser secondUser);
        
        /// <summary>
        /// Get messages from conversation.
        /// </summary>
        /// <param name="conversationId">Id of conversation from which messages are loaded.</param>
        /// <param name="count">Number of most recent messages.</param>
        /// <returns>List of messages from conversation</returns>
        IEnumerable<Message> GetMessages(int conversationId, int count = 10);
        
        /// <summary>
        /// Checks if conversation exists.
        /// </summary>
        /// <param name="conversationId">Conversation id to check if exists.</param>
        /// <returns>Bool representing existance of conversation.</returns>
        bool ConversationExists (int conversationId);
        
        /// <summary>
        /// Gets all participants in conversation.
        /// </summary>
        /// <param name="conversationId">Conversation from which participants are loaded.</param>
        /// <returns>List of participants in the conversation.</returns>
        IEnumerable<ApplicationUser>? GetParticipants(int conversationId);
    }
}
