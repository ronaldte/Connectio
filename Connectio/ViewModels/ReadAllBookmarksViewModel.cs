using Connectio.Models;

namespace Connectio.ViewModels
{
    public class ReadAllBookmarksViewModel
    {
        public IEnumerable<ReadPostViewModel> BookmarkedPosts { get; set; }

        public ReadAllBookmarksViewModel(List<Bookmark> bookmarks)
        {
            List<ReadPostViewModel> bookmarkedPosts = new();
            foreach(var bookmark in bookmarks)
            {
                var header = $"Bookmarked on {bookmark.Created:d MMM yyyy}";
                bookmarkedPosts.Add(new ReadPostViewModel(bookmark.Post, header));
            }
            
            BookmarkedPosts = bookmarkedPosts;
        }
    }
}
