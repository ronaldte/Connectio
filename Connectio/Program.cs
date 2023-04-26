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

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.User.RequireUniqueEmail = true)
    .AddEntityFrameworkStores<ConnectioDbContext>()
    .AddSignInManager<EmailSignInManager>();
builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 0;
});

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseAuthentication();

app.MapDefaultControllerRoute();

app.MapRazorPages();

app.Run();
