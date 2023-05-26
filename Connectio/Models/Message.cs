namespace Connectio.Models
{
    public class Message
    {
        public int Id { get; set; }
        public ApplicationUser CreatedBy { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public string Text { get; set; } = null!;
        public Conversation Conversation { get; set; } = null!;
    }
}
