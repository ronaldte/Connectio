using Connectio.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Connectio.Data
{
    public static class DbInitializer
    {
        public static async Task Seed(IApplicationBuilder applicationBuilder)
        {
            ConnectioDbContext context = applicationBuilder.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<ConnectioDbContext>();

            if (!context.Users.Any())
            {
                context.Database.EnsureCreated();

                await AddApplicationUsers(context);

                context.SaveChanges();
            }
        }

        private async static Task AddApplicationUsers(ConnectioDbContext context)
        {
            var user1 = new ApplicationUser()
            {
                UserName = "User1",
                NormalizedUserName = "USER1",
                Created = DateTime.UtcNow,
                DisplayName = "User1",
                Email = "user1@seed.com",
                NormalizedEmail = "USER1@SEED.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                Verified = true
            };
            var user2 = new ApplicationUser()
            {
                UserName = "User2",
                NormalizedUserName = "USER2",
                Created = DateTime.UtcNow,
                DisplayName = "User2",
                Email = "user2@seed.com",
                NormalizedEmail = "USER2@SEED.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var user3 = new ApplicationUser()
            {
                UserName = "User3",
                NormalizedUserName = "USER3",
                Created = DateTime.UtcNow,
                DisplayName = "User3",
                Email = "user3@seed.com",
                NormalizedEmail = "USER3@SEED.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString(),
                Verified = true
            };
            var user4 = new ApplicationUser()
            {
                UserName = "User4",
                NormalizedUserName = "USER4",
                Created = DateTime.UtcNow,
                DisplayName = "User4",
                Email = "user4@seed.com",
                NormalizedEmail = "USER4@SEED.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };
            var user5 = new ApplicationUser()
            {
                UserName = "User5",
                NormalizedUserName = "USER5",
                Created = DateTime.UtcNow,
                DisplayName = "User5",
                Email = "user5@seed.com",
                NormalizedEmail = "USER5@SEED.COM",
                EmailConfirmed = true,
                LockoutEnabled = false,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var users = new List<ApplicationUser>() { user1, user2, user3, user4, user5 };

            foreach (var user in users)
            {
                var password = new PasswordHasher<ApplicationUser>();
                user.PasswordHash = password.HashPassword(user, "password");
                var userStore = new UserStore<ApplicationUser>(context);
                await userStore.CreateAsync(user);
            }
        }
    }
}
