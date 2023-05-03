using Connectio.Models;

namespace Connectio.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly ConnectioDbContext _dbContext;

        public UserRepository(ConnectioDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public ApplicationUser? GetUserByUserName(string username)
        {
            return _dbContext.Users.Where(u => u.UserName == username).FirstOrDefault();
        }
    }
}
