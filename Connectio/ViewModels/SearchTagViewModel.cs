namespace Connectio.ViewModels
{
    /// <summary>
    /// SearchTag model represents search result for the tag
    /// </summary>
    public class SearchTagViewModel
    {
        
        /// <summary>
        /// Searched tag.
        /// </summary>
        public string SearchKeyword { get; set; }
        
        /// <summary>
        /// List tagged with the tag.
        /// </summary>
        public IEnumerable<ReadTagViewModel> Tags { get; set; }

        public SearchTagViewModel(string searchKeyword, IEnumerable<ReadTagViewModel> tags)
        {
            SearchKeyword = searchKeyword;
            Tags = tags;
        }
    }
}
