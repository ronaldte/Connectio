namespace Connectio.ViewModels
{
    public class SearchUserViewModel
    {
        public string SearchKeyword { get; set; }
        public IEnumerable<ReadUserViewModel> Users { get; set; }

        public SearchUserViewModel(string searchKeyword, IEnumerable<ReadUserViewModel> users)
        {
            SearchKeyword = searchKeyword;
            Users = users;
        }
    }
}
