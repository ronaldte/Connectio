using Connectio.Models;

namespace Connectio.Data
{
    public interface IConversationRepository
    {
        void SaveChanges();
        void CreateConversation(Conversation conversation);
        void CreateMessage(Message message);
    }
}
