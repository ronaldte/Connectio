using Connectio.Models;

namespace Connectio.ViewModels
{
    /// <summary>
    /// ReadTag model represent tag entity.
    /// </summary>
    public class ReadTagViewModel
    {
        /// <summary>
        /// Id of the tag.
        /// </summary>
        public int Id { get; set; }
        
        /// <summary>
        /// Name (string value) of the tag.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Number of posts which are tagged by this tag.
        /// </summary>
        public int PostsCount { get; set; }

        public ReadTagViewModel(Tag tag)
        {
            Id = tag.Id;
            Name = tag.Name;
            PostsCount = tag.Posts.Count;
        }
    }
}
