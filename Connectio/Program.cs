using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Connectio.Areas.Identity;
using Connectio.Data;
using Connectio.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ConnectioDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectioDbContextConnection") ?? throw new InvalidOperationException("\"Connection string 'ConnectioDbContextConnection' not found.\""));
});

builder.Services.AddDefaultIdentity<ApplicationUser>(options =>
{
    options.User.RequireUniqueEmail = true;
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
})
    .AddEntityFrameworkStores<ConnectioDbContext>()
    .AddSignInManager<EmailOrUserNameSignInManager>();

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
});

builder.Services.AddScoped<IPostRepository, PostRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IReactionRepository, ReactionRepository>();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "Post",
    pattern: "{controller=Post}/{action=Index}/{id:int}"
);

app.MapControllerRoute(
    name: "User",
    pattern: "{controller=User}/{action=Index}/{username}"
);

app.MapControllerRoute(
    name: "Explore",
    pattern: "{controller=Explore}/{action=Index}"
);
app.MapRazorPages();

app.Run();
