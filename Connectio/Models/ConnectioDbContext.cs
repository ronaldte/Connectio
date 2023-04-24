using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Connectio.Models
{
    public class ConnectioDbContext : IdentityDbContext
    {
        public ConnectioDbContext(DbContextOptions<ConnectioDbContext> options) : base(options)
        {  
        }

        public DbSet<PostModel> Posts { get; set; }
    }
}
