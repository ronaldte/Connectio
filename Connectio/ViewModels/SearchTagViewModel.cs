namespace Connectio.ViewModels
{
    public class SearchTagViewModel
    {
        public string SearchKeyword { get; set; }
        public IEnumerable<ReadTagViewModel> Tags { get; set; }

        public SearchTagViewModel(string searchKeyword, IEnumerable<ReadTagViewModel> tags)
        {
            SearchKeyword = searchKeyword;
            Tags = tags;
        }
    }
}
