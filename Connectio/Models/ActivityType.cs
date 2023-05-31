namespace Connectio.Models
{
    /// <summary>
    /// Represents type of post activity made by user.
    /// </summary>
    public enum ActivityType
    {
        /// <summary>
        /// Represents new post created by user.
        /// </summary>
        New,
        
        /// <summary>
        /// Represents liked post by user.
        /// </summary>
        Like,
        
        /// <summary>
        /// Represents comment made on post by user.
        /// </summary>
        Comment,

        /// <summary>
        /// Represents bookmarked post by user.
        /// </summary>
        Bookmark
    }
}
