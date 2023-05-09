using Connectio.Models;

namespace Connectio.Data
{
    public interface IReactionRepository
    {
        Bookmark? GetBookmark(ApplicationUser user, Post post);
        void CreateBookmark(Bookmark bookmark);
        void DeleteBookmark(Bookmark bookmark);
        List<Bookmark> GetAllBookmarks(ApplicationUser user);
        PostReactions GetReactions(ApplicationUser user, Post post);
    }
}
