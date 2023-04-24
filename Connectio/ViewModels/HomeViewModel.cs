using Connectio.ViewModels.Post;

namespace Connectio.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<PostReadViewModel> Posts { get; }

        public HomeViewModel(IEnumerable<PostReadViewModel> posts)
        {
            Posts = posts;
        }
    }
}
