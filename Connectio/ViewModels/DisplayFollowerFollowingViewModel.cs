using Connectio.Models;

namespace Connectio.ViewModels
{
    /// <summary>
    /// Display list of users who follow or are being follow by the user.
    /// </summary>
    public class DisplayFollowerFollowingViewModel
    {
        /// <summary>
        /// User of whom to display followers/followings
        /// </summary>
        public ReadUserViewModel User { get; set; }
        
        /// <summary>
        /// List of followers/following
        /// </summary>
        public List<ReadUserViewModel> Users { get; set; }
        
        public DisplayFollowerFollowingViewModel(ApplicationUser user, List<ApplicationUser> users)
        {
            User = new ReadUserViewModel(user);
            
            Users = new List<ReadUserViewModel>();
            foreach(var user_accounts in users)
            {
                Users.Add(new ReadUserViewModel(user_accounts));
            }
        }

    }
}
