namespace Connectio.ViewModels
{
    /// <summary>
    /// SearchUser model represents result for searching the keyword in users.
    /// </summary>
    public class SearchUserViewModel
    {
        /// <summary>
        /// Keyword used to search among users.
        /// </summary>
        public string SearchKeyword { get; set; }
        
        /// <summary>
        /// List of users matching serach criteria.
        /// </summary>
        public IEnumerable<ReadUserViewModel> Users { get; set; }

        public SearchUserViewModel(string searchKeyword, IEnumerable<ReadUserViewModel> users)
        {
            SearchKeyword = searchKeyword;
            Users = users;
        }
    }
}
