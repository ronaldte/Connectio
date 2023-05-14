using Connectio.Models;

namespace Connectio.ViewModels
{
    public class ReadReactionViewModel
    {
        public int PostId { get; set; }
        public int CommentsCount { get; set; } = 0;
        public int LikesCount { get; set; } = 0;
        public int BookmarksCount { get; set; } = 0;
        public bool Liked { get; set; } = false;
        public bool Bookmarked { get; set; } = false;
    }
}
