using Connectio.Models;

namespace Connectio.ViewModels
{
    /// <summary>
    /// ReadAllBookmarks model represents list of bookmarked posts.
    /// </summary>
    public class ReadAllBookmarksViewModel
    {
        /// <summary>
        /// Listof bookmarked posts.
        /// </summary>
        public IEnumerable<ReadPostViewModel> BookmarkedPosts { get; set; }

        public ReadAllBookmarksViewModel(List<Bookmark> bookmarks)
        {
            List<ReadPostViewModel> bookmarkedPosts = new();
            foreach(var bookmark in bookmarks)
            {
                var header = $"Bookmarked on {bookmark.Created:d MMM yyyy}";
                bookmarkedPosts.Add(new ReadPostViewModel(bookmark.Post) { Header = header, ActivityType=ActivityType.Bookmark});
            }
            
            BookmarkedPosts = bookmarkedPosts;
        }
    }
}
