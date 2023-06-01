namespace Connectio.ViewModels
{
    /// <summary>
    /// Search model represents short result of posts, users and tags that contain searched keyword.
    /// </summary>
    public class SearchViewModel
    {
        /// <summary>
        /// Keyword used to search.
        /// </summary>
        public string SearchKeyword { get; set; } = string.Empty;
        
        /// <summary>
        /// List of Posts contining the searched keyword.
        /// </summary>
        public IEnumerable<ReadPostViewModel> Posts { get; set; }
        
        /// <summary>
        /// List of Users containing the searched keyword.
        /// </summary>
        public IEnumerable<ReadUserViewModel> Users { get; set; }
        
        /// <summary>
        /// List of Tags which match the searched keyword.
        /// </summary>
        public IEnumerable<ReadTagViewModel> Tags { get; set; }

        /// <summary>
        /// Number of posts satisfying the search criteria.
        /// </summary>
        public int TotalFoundPosts { get; set; }

        /// <summary>
        /// Number of users satisfying the serach criteria.
        /// </summary>
        public int TotalFoundUsers { get; set; }

        /// <summary>
        /// Number of tags satisfying the serach criteria.
        /// </summary>
        public int TotalFoundTags { get; set; }

        public SearchViewModel(string searchKeyword, IEnumerable<ReadPostViewModel> posts, IEnumerable<ReadUserViewModel> users, IEnumerable<ReadTagViewModel> tags, int totalFoundPosts, int totalFoundUsers, int totalFoundTags)
        {
            SearchKeyword = searchKeyword;
            Posts = posts;
            Users = users;
            Tags = tags;
            TotalFoundPosts = totalFoundPosts;
            TotalFoundUsers = totalFoundUsers;
            TotalFoundTags = totalFoundTags;
        }
    }
}
