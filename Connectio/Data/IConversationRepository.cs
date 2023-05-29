using Connectio.Models;

namespace Connectio.Data
{
    public interface IConversationRepository
    {
        void SaveChanges();
        void CreateConversation(Conversation conversation);
        void CreateMessage(Message message);
        IEnumerable<Conversation> GetUserConversations(ApplicationUser user);
        Conversation? GetConversation(ApplicationUser firstUser, ApplicationUser secondUser);
        IEnumerable<Message> GetMessages(int conversationId, int count = 10);
        bool ConversationExists (int conversationId);
        IEnumerable<ApplicationUser>? GetParticipants(int conversationId);
    }
}
