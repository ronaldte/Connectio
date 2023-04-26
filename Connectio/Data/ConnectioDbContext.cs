using Connectio.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Connectio.Data
{
    public class ConnectioDbContext : IdentityDbContext
    {
        public ConnectioDbContext(DbContextOptions<ConnectioDbContext> options) : base(options)
        {
        }

        public DbSet<PostModel> Posts { get; set; }
    }
}
