using Connectio.Models;
using Microsoft.EntityFrameworkCore;

namespace Connectio.Data
{
    /// <inheritdoc/>
    public class UserRepository : IUserRepository
    {
        private readonly ConnectioDbContext _dbContext;

        public UserRepository(ConnectioDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        /// <inheritdoc/>
        public void UpdateFollower(ApplicationUser following)
        {
            _dbContext.Users.Update(following);
            _dbContext.SaveChanges();
        }

        /// <inheritdoc/>
        public ApplicationUser? GetUserByUserName(string username)
        {
            return _dbContext.Users
                .Where(u => u.UserName == username)
                .Include(u => u.Followers)
                .Include(u => u.Following)
                .FirstOrDefault();
        }

        /// <inheritdoc/>
        public void UpdateProfilePicture(ApplicationUser user, string? fileName)
        {
            if(fileName == null)
            {
                user.HasDefaultProfilePicture = true;
                user.ProfilePictureUrl = string.Empty;
            }
            else
            {
                user.HasDefaultProfilePicture = false;
                user.ProfilePictureUrl = fileName;
            }
            
            _dbContext.SaveChanges();
        }

        /// <inheritdoc/>
        public void UpdateBannerPicture(ApplicationUser user, string? fileName)
        {
            if (fileName == null)
            {
                user.HasDefaultBannerPicture = true;
                user.BannerPictureUrl = string.Empty;
            }
            else
            {
                user.HasDefaultBannerPicture = false;
                user.BannerPictureUrl = fileName;
            }
            
            _dbContext.SaveChanges();
        }
    }
}
