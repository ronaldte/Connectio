using Connectio.Models;

namespace Connectio.ViewModels
{
    public class ReadTagViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int PostsCount { get; set; }

        public ReadTagViewModel(Tag tag)
        {
            Id = tag.Id;
            Name = tag.Name;
            PostsCount = tag.Posts.Count;
        }
    }
}
