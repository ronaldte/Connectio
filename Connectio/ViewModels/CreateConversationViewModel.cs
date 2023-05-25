using System.ComponentModel.DataAnnotations;

namespace Connectio.ViewModels
{
    public class CreateConversationViewModel
    {
        [Required]
        [Display(Name = "User name")]
        [StringLength(24, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
        public string ToUser { get; set; } = null!;

        [Required]
        [Display(Name = "Message")]
        [StringLength(512, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string MessageText { get; set; } = string.Empty;
    }
}
