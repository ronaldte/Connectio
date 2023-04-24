using Connectio.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ConnectioDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectioDbContextConnection") ?? throw new InvalidOperationException("\"Connection string 'ConnectioDbContextConnection' not found.\""));
});

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false).AddEntityFrameworkStores<ConnectioDbContext>();

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
