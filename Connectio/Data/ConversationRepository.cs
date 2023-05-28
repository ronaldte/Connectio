using Connectio.Models;
using Microsoft.EntityFrameworkCore;

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

        public IEnumerable<Conversation> GetUserConversations(ApplicationUser user)
        {
            return _dbContext.Conversations.Where(c => c.Participants.Contains(user)).Include(c => c.Participants).ToList();
        }

        public IEnumerable<Message> GetMessages(int conversationId, int count = 10)
        {
            var conversation = _dbContext.Conversations
                .Where(c => c.Id == conversationId)
                .Include(c => c.Messages)
                .ThenInclude(m => m.CreatedBy)
                .First();

            return conversation.Messages.TakeLast(count).ToList();
        }

        public bool ConversationExists(int conversationId)
        {
            return _dbContext.Conversations.Any(c => c.Id == conversationId);
        }
    }
}
