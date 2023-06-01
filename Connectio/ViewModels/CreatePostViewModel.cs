using System.ComponentModel.DataAnnotations;

namespace Connectio.ViewModels
{
    /// <summary>
    /// CreatePost represents model for creating new post.
    /// </summary>
    public class CreatePostViewModel
    {
        /// <summary>
        /// Body text content of post to create.
        /// </summary>
        [Required]
        [StringLength(256, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 1)]
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// First image in the post.
        /// </summary>
        public IFormFile? ImageFile1 { get; set; }
        
        /// <summary>
        /// Second image in the post.
        /// </summary>
        public IFormFile? ImageFile2 { get; set; }
        
        /// <summary>
        /// Third image in the post.
        /// </summary>
        public IFormFile? ImageFile3 { get; set; }
    }
}
