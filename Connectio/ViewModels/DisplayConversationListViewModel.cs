using Connectio.Models;

namespace Connectio.ViewModels
{
    public class DisplayConversationListViewModel
    {
        public int Id { get; set; }
        public bool IsPrivate { get; set; }
        public List<ReadUserViewModel> Participants { get; set; } = new();
        public Message LastMessage { get; set; } = null!;
    }
}
