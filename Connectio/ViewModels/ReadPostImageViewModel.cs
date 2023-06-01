using Connectio.Models;

namespace Connectio.ViewModels
{
    /// <summary>
    /// ReadPostImage model represents model for image content in the post.
    /// </summary>
    public class ReadPostImageViewModel
    {
        /// <summary>
        /// Storage location of the image.
        /// </summary>
        public string ImageUrl { get; set; } = null!;
        
        /// <summary>
        /// Place in the sequence of the image.
        /// </summary>
        public int Order { get; set; } = 0;

        public ReadPostImageViewModel(PostImage image)
        {
            ImageUrl = image.ImageUrl;
            Order = image.Order;
        }
    }
}
