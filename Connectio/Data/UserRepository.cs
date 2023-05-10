using Connectio.Models;
using Microsoft.EntityFrameworkCore;

namespace Connectio.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ConnectioDbContext _dbContext;

        public UserRepository(ConnectioDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddFollower(ApplicationUser following)
        {
            _dbContext.Users.Update(following);
            _dbContext.SaveChanges();
        }

        public ApplicationUser? GetUserByUserName(string username)
        {
            return _dbContext.Users.
                Where(u => u.UserName == username)
                .Include(u => u.Followers)
                .Include(u => u.Following)
                .FirstOrDefault();
        }
    }
}
