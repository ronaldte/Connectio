using Connectio.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

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

                AddTags(context);
                context.SaveChanges();

                AddPosts(context);
                context.SaveChanges();

                AddLikes(context);
                context.SaveChanges();

                AddBookmarks(context);
                context.SaveChanges();

                AddComments(context);
                context.SaveChanges();

                AddFollowers(context);
                context.SaveChanges();

                UpdateRandomness(context);
                context.SaveChanges();

                AddConversations(context);
                context.SaveChanges();

                AddImagesToPost(context);
                context.SaveChanges();
            }
        }

        private async static Task AddApplicationUsers(ConnectioDbContext context)
        {
            var users = new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    UserName = "Vegetarian",
                    NormalizedUserName = "VEGETARIAN",
                    Created = DateTime.UtcNow,
                    DisplayName = "Hermann P. Schnitzel",
                    Email = "vegetarian@seed.com",
                    NormalizedEmail = "VEGETARIAN@SEED.COM",
                    Description = "Integer in sapien. Aliquam erat volutpat. Et harum quidem rerum facilis est et expedita distinctio",
                    Location = "Nulla est",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Verified = true
                },
                new ApplicationUser()
                {
                    UserName = "Cyclist",
                    NormalizedUserName = "CYCLIST",
                    Created = DateTime.UtcNow,
                    DisplayName = "Chauffina Carr",
                    Email = "cyclist@seed.com",
                    NormalizedEmail = "CYCLIST@SEED.COM",
                    Description = "Fusce aliquam vestibulum ipsum.",
                    Url = "google.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new ApplicationUser()
                {
                    UserName = "Manager",
                    NormalizedUserName = "MANAGER",
                    Created = DateTime.UtcNow,
                    DisplayName = "Miles Tone",
                    Email = "manager@seed.com",
                    NormalizedEmail = "MANAGER@SEED.COM",
                    Description = "Fusce tellus odio, dapibus id fermentum quis, suscipit id erat. Mauris tincidunt sem sed arcu. Aliquam erat volutpat.",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString(),
                    Verified = true
                },
                new ApplicationUser()
                {
                    UserName = "nomad",
                    NormalizedUserName = "NOMAD",
                    Created = DateTime.UtcNow,
                    DisplayName = "Gunther Beard",
                    Email = "nomad@seed.com",
                    NormalizedEmail = "NOMAD@SEED.COM",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new ApplicationUser()
                {
                    UserName = "void",
                    NormalizedUserName = "VOID",
                    Created = DateTime.UtcNow,
                    DisplayName = "Bodrum Salvador",
                    Email = "VOID@seed.com",
                    NormalizedEmail = "VOID@SEED.COM",
                    Description = "The Null Void, an alternate dimension where the galaxy's worst of the worst are banished.",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                },                
                new ApplicationUser()
                {
                    UserName = "FitnessFanatic",
                    NormalizedUserName = "FITNESSFANATIC",
                    Created = DateTime.UtcNow,
                    DisplayName = "Ingredia Nutrisha",
                    Email = "fitnessfanatic@seed.com",
                    NormalizedEmail = "FITNESSFANATIC@SEED.COM",
                    Description = "Banging weights and slamming plates 🏋️",
                    Url = "goldsgym.com",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new ApplicationUser()
                {
                    UserName = "green",
                    NormalizedUserName = "GREEN",
                    Created = DateTime.UtcNow,
                    DisplayName = "Indigo Violet",
                    Email = "green@seed.com",
                    NormalizedEmail = "GREEN@SEED.COM",
                    Description = "Favourite color: green",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                },
                new ApplicationUser()
                {
                    UserName = "knitter",
                    NormalizedUserName = "KNITTER",
                    Created = DateTime.UtcNow,
                    DisplayName = "Nathaneal Down",
                    Email = "knitter@seed.com",
                    NormalizedEmail = "KNITTER@SEED.COM",
                    Description = "Fusce nibh. Maecenas fermentum, sem in pharetra pellentesque, velit turpis volutpat ante, in pharetra metus odio a lectus. Sed elit dui, pellentesque a, faucibus vel, interdum nec, diam.",
                    EmailConfirmed = true,
                    LockoutEnabled = false,
                    SecurityStamp = Guid.NewGuid().ToString()
                }
            };

            foreach (var user in users)
            {
                var password = new PasswordHasher<ApplicationUser>();
                user.PasswordHash = password.HashPassword(user, "password");
                var userStore = new UserStore<ApplicationUser>(context);
                await userStore.CreateAsync(user);
            }
        }

        private static void AddTags(ConnectioDbContext context)
        {
            var tags = new List<Tag>()
            {
                new Tag {Name = "Lorem"},
                new Tag {Name = "Ipsum"},
                new Tag {Name = "Dolor"},
                new Tag {Name = "Sit"},
                new Tag {Name = "Amet"}
            };
            context.Tags.AddRange(tags);
        }

        private static void AddPosts(ConnectioDbContext context)
        {
            var users = context.Users.ToList();
            var tags = context.Tags.ToList();
            var posts = new List<Post>()
            {
                new Post(){ Created = new DateTime(2023, 6, 8, 8, 30, 0), User = users[0], Tags = new List<Tag>(){tags[0], tags[1], tags[2], tags[3], tags[4]} , Text="#Lorem #Ipsum #Dolor #Sit #Amet, consectetuer adipiscing elit. Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet ut et voluptates repudiandae sint et molestiae non recusandae." },
                new Post(){ Created = new DateTime(2023, 6, 8, 9, 31, 0), User = users[7], Tags = new List<Tag>(){tags[0], tags[1]}, Text="Aliquam erat volutpat. #Lorem #Ipsum"},
                new Post(){ Created = new DateTime(2023, 6, 8, 10, 32, 0), User = users[2], Text="Nullam dapibus fermentum ipsum. Nullam eget nisl. Proin pede metus, vulputate nec, fermentum fringilla, vehicula vitae, justo. Sed convallis magna eu sem. Proin in tellus sit amet nibh dignissim sagittis. Vivamus porttitor turpis ac leo."},
                new Post(){ Created = new DateTime(2023, 6, 8, 11, 33, 0), User = users[3], Text="Nullam sapien sem, ornare ac, nonummy non, lobortis a enim. In enim a arcu imperdiet malesuada. Integer tempor."},
                new Post(){ Created = new DateTime(2023, 6, 8, 12, 34, 0), User = users[4], Tags = new List<Tag>(){tags[0], tags[4]}, Text="Temporibus autem quibusdam et aut officiis #Lorem #Amet debitis aut rerum necessitatibus saepe eveniet ut et voluptates repudiandae sint et molestiae non recusandae."},

                new Post(){ Created = new DateTime(2023, 6, 8, 13, 30, 0), User = users[4], Tags = new List<Tag>(){ tags[4] }, Text = "Fusce wisi. #Amet Fusce aliquam vestibulum ipsum. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus."},
                new Post(){ Created = new DateTime(2023, 6, 8, 14, 31, 0), User = users[5], Tags = new List<Tag>(){ tags[3], tags[2] }, Text = "#Dolor Mauris dolor felis, sagittis at, luctus sed, aliquam non, tellus. Praesent in mauris eu tortor porttitor accumsan. #Sit #Sit #Sit"},
                new Post(){ Created = new DateTime(2023, 6, 8, 15, 32, 0), User = users[2], Tags = new List<Tag>(){ tags[2] }, Text = "In enim a arcu imperdiet malesuada. #Dolor"},
                new Post(){ Created = new DateTime(2023, 6, 8, 16, 33, 0), User = users[6], Tags = new List<Tag>(){ tags[1], tags[2] }, Text = "Integer pellentesque quam vel velit. #Dolor #Ipsum Temporibus autem quibusdam et aut officiis debitis aut rerum necessitatibus saepe eveniet ut et voluptates repudiandae sint et molestiae non recusandae."},
                new Post(){ Created = new DateTime(2023, 6, 8, 17, 34, 0), User = users[0], Tags = new List<Tag>(){ tags[0] }, Text = "Aliquam erat volutpat. Ut tempus purus at lorem. \n #Lorem"}
            };
            context.Posts.AddRange(posts);
        }

        private static void AddLikes(ConnectioDbContext context)
        {
            var users = context.Users.ToList();
            var posts = context.Posts.ToList();
            var rnd = new Random();

            foreach(var post in posts)
            {
                post.LikedBy.AddRange(users.OrderBy(u => rnd.Next()).Take(rnd.Next(users.Count)));
            }
        }

        private static void AddBookmarks(ConnectioDbContext context)
        {
            var users = context.Users.ToList();
            var posts = context.Posts.ToList();
            var rnd = new Random();

            foreach(var user in users)
            {
                var postsToBookmark = posts.OrderBy(p => rnd.Next()).Take(rnd.Next(posts.Count/2));
                user.BookmarkedPosts.AddRange(postsToBookmark);
            }
        }

        private static void AddComments(ConnectioDbContext context)
        {
            var users = context.Users.ToList();
            var posts = context.Posts.ToList();
            var comments = new List<string>()
            {
                "Suspendisse sagittis ultrices augue.",
                "Sed elit dui, pellentesque a, faucibus vel, interdum nec, diam.",
                "Praesent vitae arcu tempor neque lacinia pretium.",
                "Duis viverra diam non justo.",
                "Nam sed tellus id magna elementum tincidunt.",
                "Vivamus porttitor turpis ac leo.",
                "Integer rutrum, orci vestibulum ullamcorper ultricies, lacus quam ultricies odio, vitae placerat pede sem sit amet enim.",
                "Mauris metus.",
                "Etiam ligula pede, sagittis quis, interdum ultricies, scelerisque eu.",
                "Aenean id metus id velit ullamcorper pulvinar."
            };
            var rnd = new Random();

            for(int round = 0; round < 10; round++)
            {
                foreach (var user in users)
                {
                    posts[rnd.Next(0, posts.Count)].Comments.Add(new Comment() { User = user, Text = comments[rnd.Next(0, comments.Count)] });
                }
            }
        }

        private static void AddFollowers(ConnectioDbContext context)
        {
            var users = context.Users.ToList();
            var rnd = new Random();

            foreach(var user in users)
            {
                var usersToFollow = users.Where(u => u != user).OrderBy(u => rnd.Next(0, users.Count - 1)).Take(rnd.Next(0, users.Count - 1));
                user.Following.AddRange(usersToFollow);
            }
        }

        private static void UpdateRandomness(ConnectioDbContext context)
        {
            var rnd = new Random();
            var max_minutes = 1440;
            
            var likes = context.Likes.ToList();
            foreach(var like in likes)
            {
                like.Created = like.Created.AddMinutes(-rnd.Next(max_minutes));
            }

            var bookmarks = context.Bookmarks.ToList();
            foreach(var bookmark in bookmarks)
            {
                bookmark.Created = bookmark.Created.AddMinutes(-rnd.Next(max_minutes));
            }

            var comments = context.Comments.ToList();
            foreach (var comment in comments)
            {
                comment.Created = comment.Created.AddMinutes(-rnd.Next(max_minutes));
            }
        }

        private static void AddConversations(ConnectioDbContext context)
        {
            var users = context.Users.ToList();
            var rnd = new Random();

            var messages = new List<string>()
            {
                "Donec ipsum massa, ullamcorper in, auctor et, scelerisque sed, est. Ut tempus purus at lorem. Phasellus enim erat, vestibulum vel, aliquam a, posuere eu, velit. Cras elementum. Mauris dolor felis, sagittis at, luctus sed, aliquam non, tellus.",
                "Phasellus faucibus molestie nisl",
                "Nam sed tellus id magna elementum tincidunt",
                "Integer vulputate sem a nibh rutrum consequat",
                "Mauris elementum mauris vitae tortor",
                "Duis sapien nunc, commodo et, interdum suscipit, sollicitudin et, dolor. Nulla est. Praesent vitae arcu tempor neque lacinia pretium. Nulla accumsan, elit sit amet varius semper, nulla mauris mollis quam, tempor suscipit diam nulla vel leo",
                "Maecenas ipsum velit, consectetuer eu lobortis ut, dictum at dui. Integer lacinia",
                "Suspendisse sagittis ultrices augue. Nulla turpis magna, cursus sit amet, suscipit a, interdum id, felis.",
                "Nam sed tellus id magna elementum tincidunt. Aliquam ante. Curabitur bibendum justo non orci. Nemo enim ipsam voluptatem quia voluptas sit aspernatur aut odit aut fugit"
            };
            var conversations = new List<Conversation>();

            for (int i = 0; i < 10; i++)
            {
                var conversationBetween = users.OrderBy(u => rnd.Next(0, users.Count - 1)).Take(2).ToList();

                if (conversations.Any(c => c.Participants.Contains(conversationBetween[0]) && c.Participants.Contains(conversationBetween[1])))
                {
                    continue;
                }

                var conversation = new Conversation() { IsPrivate = true, Participants = conversationBetween };

                for (int j = 0; j < 10; j++)
                {
                    var msgSender = conversationBetween[rnd.Next(0, 2)];
                    var message = new Message() { Conversation = conversation, CreatedBy = msgSender, Text = messages[rnd.Next(messages.Count)] };
                    conversation.Messages.Add(message);
                }

                conversations.Add(conversation);
            }

            context.Conversations.AddRange(conversations);
        }

        private static void AddImagesToPost(ConnectioDbContext context)
        {
            var placeholderImage = "fb2e28ac-86e6-4ac8-9531-fe4a37e59000.png";
            var rnd = new Random();
            var posts = context.Posts.ToList().OrderBy(p => rnd.Next()).Take(context.Posts.Count()/2);

            foreach(var post in posts)
            {
                var numberOfImages = rnd.Next(4);
                for(int j=0; j<numberOfImages; j++)
                {
                    post.PostImages.Add(new PostImage() { ImageUrl = placeholderImage, Order = j, Post = post });
                }
            }
        }
    }
}
