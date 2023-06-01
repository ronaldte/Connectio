namespace Connectio.ViewModels
{
    /// <summary>
    /// DisplayFollow represents if given user if follower or not.
    /// </summary>
    public class DisplayFollowViewModel
    {
        /// <summary>
        /// Username of user who is following.
        /// </summary>
        public string FollowingName { get; set; } = string.Empty;
        
        /// <summary>
        /// State if the user if following or not.
        /// </summary>
        public bool IsFollower { get; set; } = false;
    }
}
