namespace Connectio.Models
{
    public class Conversation
    {
        public int Id { get; set; }
        public bool IsPrivate { get; set; } = true;
        public List<ApplicationUser> Participants { get; set; } = new();
        public List<Message> Messages { get; set; } = new();
    }
}
