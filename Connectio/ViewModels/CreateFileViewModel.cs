using System.ComponentModel.DataAnnotations;

namespace Connectio.ViewModels
{
    /// <summary>
    /// CreateFile represents model for saving file in application.
    /// </summary>
    public class CreateFileViewModel
    {
        /// <summary>
        /// File to be saved.
        /// </summary>
        [Required]
        [Display(Name = "File")]
        public IFormFile File { get; set; } = null!;
    }
}
