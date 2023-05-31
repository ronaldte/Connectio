using Connectio.Models;

namespace Connectio.Data
{
    /// <summary>
    /// Reaction repository manages user reactions on posts.
    /// </summary>
    public interface IReactionRepository
    {
        /// <summary>
        /// Gets bookmark model.
        /// </summary>
        /// <param name="user">User who created the bookmark.</param>
        /// <param name="post">Posts which was bookmarked.</param>
        /// <returns>Null or Bookmark created by user on post.</returns>
        Bookmark? GetBookmark(ApplicationUser user, Post post);
     
        /// <summary>
        /// Adds new bookmark to DB.
        /// </summary>
        /// <param name="bookmark">Bookmark to be added to DB.</param>
        void CreateBookmark(Bookmark bookmark);
        
        /// <summary>
        /// Removes bookmark from DB.
        /// </summary>
        /// <param name="bookmark">Bookmark to be removed from DB.</param>
        void DeleteBookmark(Bookmark bookmark);
        
        /// <summary>
        /// Gets all bookmarks by user.
        /// </summary>
        /// <param name="user">User who created the bookmarks.</param>
        /// <returns>List with bookmarks this user created.</returns>
        List<Bookmark> GetAllBookmarks(ApplicationUser user);
        
        /// <summary>
        /// Gets the like by user on given post.
        /// </summary>
        /// <param name="user">User who created the like.</param>
        /// <param name="post">Post which user liked.</param>
        /// <returns>Like user made on post or null.</returns>
        Like? GetLike(ApplicationUser user, Post post);
        
        /// <summary>
        /// Gets all likes made by user.
        /// </summary>
        /// <param name="user">User who created the likes.</param>
        /// <returns>List of all likes made by user.</returns>
        IEnumerable<Like> GetAllUserLikes(ApplicationUser user);
        
        /// <summary>
        /// Adds new like to DB.
        /// </summary>
        /// <param name="like">Like to be added to DB.</param>
        void CreateLike(Like like);
        
        /// <summary>
        /// Removes like from DB.
        /// </summary>
        /// <param name="like">Like to be removed from DB.</param>
        void DeleteLike(Like like);
        
        /// <summary>
        /// Adds new comment to DB.
        /// </summary>
        /// <param name="comment">Commments to be added to DB.</param>
        void CreateComment(Comment comment);
        
        /// <summary>
        /// Deletes comment from DB.
        /// </summary>
        /// <param name="comment">Comment to be removed from DB.</param>
        void DeleteComment(Comment comment);
        
        /// <summary>
        /// Gets all comments on post.
        /// </summary>
        /// <param name="post">Post from which comments to load.</param>
        /// <returns>List of all comments on a post.</returns>
        List<Comment> GetAllCommentsOnPost(Post post);
        
        /// <summary>
        /// Gets all comments made by user.
        /// </summary>
        /// <param name="user">User whos comments to be loaded.</param>
        /// <returns>List with all comments made by user.</returns>
        IEnumerable<Comment> GetAllUserComments(ApplicationUser user);
    }
}
