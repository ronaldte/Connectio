namespace Connectio.ViewModels
{
    public class SearchPostViewModel
    {
        public string SearchKeyword { get; set; }
        public IEnumerable<ReadPostViewModel> Posts { get; set; }

        public SearchPostViewModel(string searchKeyword, IEnumerable<ReadPostViewModel> posts)
        {
            SearchKeyword = searchKeyword;
            Posts = posts;
        }
    }
}
