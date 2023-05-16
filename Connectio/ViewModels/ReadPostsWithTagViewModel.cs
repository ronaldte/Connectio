namespace Connectio.ViewModels
{
    public class ReadPostsWithTagViewModel
    {
        public ReadTagViewModel Tag { get; set; }
        public IEnumerable<ReadPostViewModel> Posts { get; set; }

        public ReadPostsWithTagViewModel(ReadTagViewModel tag, IEnumerable<ReadPostViewModel> posts)
        {
            Tag = tag;
            Posts = posts;
        }
    }
}
