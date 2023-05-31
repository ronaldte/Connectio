using Connectio.Models;
using Microsoft.EntityFrameworkCore;

namespace Connectio.Data
{
    /// <inheritdoc/>
    public class ConversationRepository : IConversationRepository
    {
        private readonly ConnectioDbContext _dbContext;

        public ConversationRepository(ConnectioDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <inheritdoc/>
        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        /// <inheritdoc/>
        public void CreateConversation(Conversation conversation)
        {
            _dbContext.Conversations.Add(conversation);
        }

        /// <inheritdoc/>
        public void CreateMessage(Message message)
        {
            _dbContext.Messages.Add(message);
        }

        /// <inheritdoc/>
        public IEnumerable<Conversation> GetUserConversations(ApplicationUser user)
        {
            return _dbContext.Conversations.Where(c => c.Participants.Contains(user)).Include(c => c.Participants).ToList();
        }

        /// <inheritdoc/>
        public IEnumerable<Message> GetMessages(int conversationId, int count = 10)
        {
            var conversation = _dbContext.Conversations
                .Where(c => c.Id == conversationId)
                .Include(c => c.Messages)
                .ThenInclude(m => m.CreatedBy)
                .First();

            return conversation.Messages.TakeLast(count).ToList();
        }

        /// <inheritdoc/>
        public bool ConversationExists(int conversationId)
        {
            return _dbContext.Conversations.Any(c => c.Id == conversationId);
        }

        /// <inheritdoc/>
        public Conversation? GetConversation(ApplicationUser firstUser, ApplicationUser secondUser)
        {
            return _dbContext.Conversations
                .Where(c => c.IsPrivate == true)
                .Where(c => c.Participants.Contains(firstUser) && c.Participants.Contains(secondUser))
                .FirstOrDefault();
        }

        /// <inheritdoc/>
        public IEnumerable<ApplicationUser>? GetParticipants(int conversationId)
        {
            var conversation = _dbContext.Conversations.Where(c => c.Id == conversationId).Include(c => c.Participants).FirstOrDefault();
            return conversation?.Participants;
        }
    }
}
