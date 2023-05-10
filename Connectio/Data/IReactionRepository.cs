using Connectio.Models;

namespace Connectio.Data
{
    public interface IReactionRepository
    {
        Bookmark? GetBookmark(ApplicationUser user, Post post);
        void CreateBookmark(Bookmark bookmark);
        void DeleteBookmark(Bookmark bookmark);
        List<Bookmark> GetAllBookmarks(ApplicationUser user);
        Like? GetLike(ApplicationUser user, Post post);
        IEnumerable<Like> GetAllUserLikes(ApplicationUser user);
        void CreateLike(Like like);
        void DeleteLike(Like like);
        PostReactions GetReactions(ApplicationUser user, Post post);
        void CreateComment(Comment comment);
        void DeleteComment(Comment comment);
        List<Comment> GetAllCommentsOnPost(Post post);
        IEnumerable<Comment> GetAllUserComments(ApplicationUser user);
    }
}
