using Connectio.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Connectio.Data
{
    public class ConnectioDbContext : IdentityDbContext<ApplicationUser>
    {
        public ConnectioDbContext(DbContextOptions<ConnectioDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts { get; set; }
        public DbSet<Bookmark> Bookmarks { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Post>()
                .HasOne(e => e.User)
                .WithMany(e => e.Posts)
                .OnDelete(DeleteBehavior.ClientCascade);

            
            builder.Entity<Post>()
                .HasMany(e => e.BookmarkedBy)
                .WithMany(e => e.BookmarkedPosts)
                .UsingEntity<Bookmark>(j => j.Property(e => e.Created).HasDefaultValueSql("GETUTCDATE()"));
                
        }
    }
}
