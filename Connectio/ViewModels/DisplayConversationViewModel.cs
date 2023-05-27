namespace Connectio.ViewModels
{
    public class DisplayConversationViewModel
    {
        public int Id { get; set; }
        public bool IsPrivate { get; set; }
        public List<ReadUserViewModel> Participants { get; set; } = new();
        public List<ReadMessageViewModel> Messages { get; set; } = new();
    }
}
