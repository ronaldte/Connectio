using Connectio.Models;

namespace Connectio.Data
{
    public interface IConversationRepository
    {
        void SaveChanges();
        void CreateConversation(Conversation conversation);
        void CreateMessage(Message message);
        IEnumerable<Conversation> GetUserConversations(ApplicationUser user);
        IEnumerable<Message>? GetMessages(int conversationId, int count = 10);
    }
}
