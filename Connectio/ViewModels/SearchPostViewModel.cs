namespace Connectio.ViewModels
{
    /// <summary>
    /// SearchPosts model represents posts matching the searched keyword.
    /// </summary>
    public class SearchPostViewModel
    {
        /// <summary>
        /// Keyword which was used to find posts by.
        /// </summary>
        public string SearchKeyword { get; set; }
        
        /// <summary>
        /// List of posts containing the searched keyword.
        /// </summary>
        public IEnumerable<ReadPostViewModel> Posts { get; set; }

        public SearchPostViewModel(string searchKeyword, IEnumerable<ReadPostViewModel> posts)
        {
            SearchKeyword = searchKeyword;
            Posts = posts;
        }
    }
}
