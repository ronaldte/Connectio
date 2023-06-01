namespace Connectio.ViewModels
{
    /// <summary>
    /// ReadPostsWithTag model represents all posts which are associated with the tag.
    /// </summary>
    public class ReadPostsWithTagViewModel
    {
        /// <summary>
        /// Tag which is being used in the post.
        /// </summary>
        public ReadTagViewModel Tag { get; set; }
        
        /// <summary>
        /// Posts which use the tag.
        /// </summary>
        public IEnumerable<ReadPostViewModel> Posts { get; set; }

        public ReadPostsWithTagViewModel(ReadTagViewModel tag, IEnumerable<ReadPostViewModel> posts)
        {
            Tag = tag;
            Posts = posts;
        }
    }
}
