using Connectio.Models;

namespace Connectio.Data
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly ConnectioDbContext _dbContext;

        public ConversationRepository(ConnectioDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
        public void CreateConversation(Conversation conversation)
        {
            _dbContext.Conversations.Add(conversation);
        }

        public void CreateMessage(Message message)
        {
            _dbContext.Messages.Add(message);
        }
    }
}
