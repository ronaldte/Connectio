using System.ComponentModel.DataAnnotations;

namespace Connectio.ViewModels
{
    /// <summary>
    /// CreateConversation represents model for creating new conversation.
    /// </summary>
    public class CreateConversationViewModel
    {
        /// <summary>
        /// User, who receives the message.
        /// </summary>
        [Required]
        [Display(Name = "User name")]
        [StringLength(24, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string ToUser { get; set; } = null!;

        /// <summary>
        /// Initializing message to the user in conversation.
        /// </summary>
        [Required]
        [Display(Name = "Message")]
        [StringLength(512, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string MessageText { get; set; } = string.Empty;
    }
}
