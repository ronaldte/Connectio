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
        public DbSet<Like> Likes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Tag> Tags { get; set; }

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

            builder.Entity<Post>()
                .HasMany(e => e.LikedBy)
                .WithMany(e => e.LikedPosts)
                .UsingEntity<Like>(j => j.Property(e => e.Created).HasDefaultValueSql("GETUTCDATE()"));

            builder.Entity<Post>()
                .HasMany(e => e.CommentedBy)
                .WithMany(e => e.CommentedPosts)
                .UsingEntity<Comment>(j => j.Property(e => e.Created).HasDefaultValueSql("GETUTCDATE()"));

            builder.Entity<ApplicationUser>()
                .HasMany(e => e.Followers)
                .WithMany(e => e.Following)
                .UsingEntity(e => e.ToTable("Followers"));

            builder.Entity<Post>()
                .HasMany(e => e.Tags)
                .WithMany(e => e.Posts)
                .UsingEntity("PostTagMention");

            builder.Entity<Post>()
                .HasMany(e => e.UserMentions)
                .WithMany(e => e.PostMentions)
                .UsingEntity("PostUserMention");
        }
    }
}
