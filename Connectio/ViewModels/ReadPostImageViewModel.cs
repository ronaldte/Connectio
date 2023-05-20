using Connectio.Models;

namespace Connectio.ViewModels
{
    public class ReadPostImageViewModel
    {
        public string ImageUrl { get; set; } = null!;
        public int Order { get; set; } = 0;

        public ReadPostImageViewModel(PostImage image)
        {
            ImageUrl = image.ImageUrl;
            Order = image.Order;
        }
    }
}
