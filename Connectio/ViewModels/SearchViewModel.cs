namespace Connectio.ViewModels
{
    public class SearchViewModel
    {
        public string SearchKeyword { get; set; } = string.Empty;
        public IEnumerable<ReadPostViewModel> Posts { get; set; }
        public IEnumerable<ReadUserViewModel> Users { get; set; }
        public IEnumerable<ReadTagViewModel> Tags { get; set; }

        public SearchViewModel(string searchKeyword, IEnumerable<ReadPostViewModel> posts, IEnumerable<ReadUserViewModel> users, IEnumerable<ReadTagViewModel> tags)
        {
            SearchKeyword = searchKeyword;
            Posts = posts;
            Users = users;
            Tags = tags;
        }
    }
}
