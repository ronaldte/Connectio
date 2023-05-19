using System.ComponentModel.DataAnnotations;

namespace Connectio.ViewModels
{
    public class CreateFileViewModel
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile File { get; set; } = null!;
    }
}
