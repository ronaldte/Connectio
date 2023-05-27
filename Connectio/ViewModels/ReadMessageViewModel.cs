using Connectio.Models;

namespace Connectio.ViewModels
{
    public class ReadMessageViewModel
    {
        public int Id { get; set; }
        public ReadUserViewModel CreatedBy { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string Text { get; set; } = null!;
        public int ConversationId { get; set; }

        public ReadMessageViewModel(Message message)
        {
            Id = message.Id;
            CreatedBy = new ReadUserViewModel(message.CreatedBy);
            CreatedAt = message.CreatedAt;
            Text = message.Text;
            ConversationId = message.Conversation.Id;
        }
    }
}
