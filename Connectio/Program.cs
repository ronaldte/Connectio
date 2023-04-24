using Connectio.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ConnectioDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("ConnectioDbContextConnection") ?? throw new InvalidOperationException("\"Connection string 'ConnectioDbContextConnection' not found.\""));
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

app.MapDefaultControllerRoute();

app.Run();
