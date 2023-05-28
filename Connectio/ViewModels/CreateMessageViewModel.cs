using System.ComponentModel.DataAnnotations;

namespace Connectio.ViewModels
{
    public class CreateMessageViewModel
    {
        [Required]
        public int ConversationId { get; set; }
        [Required]
        public string MessageText { get; set; } = string.Empty;
        
    }
}
