namespace Connectio.Models
{
    /// <summary>
    /// Represents visual body content of post.
    /// </summary>
    public class PostImage
    {
        /// <summary>
        /// Key for PostImage.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Url or location of place where the image is stored.
        /// </summary>
        public string ImageUrl { get; set; } = null!;
        
        /// <summary>
        /// In case of multiple images on sinlge post, order represents arrangement of these images.
        /// </summary>
        public int Order { get; set; } = 0;
        
        /// <summary>
        /// Post of which image is part of.
        /// </summary>
        public Post Post { get; set; } = null!;
    }
}
