using Microsoft.EntityFrameworkCore;

namespace Connectio.Models
{
    public class ConnectioDbContext : DbContext
    {
        public ConnectioDbContext(DbContextOptions<ConnectioDbContext> options) : base(options)
        {  
        }

        public DbSet<PostModel> Posts { get; set; }
    }
}
