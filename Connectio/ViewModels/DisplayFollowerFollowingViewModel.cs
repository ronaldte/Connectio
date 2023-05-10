using Connectio.Models;

namespace Connectio.ViewModels
{
    public class DisplayFollowerFollowingViewModel
    {
        public ReadUserViewModel User { get; set; }
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
