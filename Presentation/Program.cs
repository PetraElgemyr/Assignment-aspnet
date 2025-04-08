using Microsoft.AspNetCore.Rewrite;
using Business;
using Hubs;
using Data.Extensions;
using Business.Handlers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();


// testa i dev om bildhantering funkar. 
if (builder.Environment.IsDevelopment())
{
    var localPath = Path.Combine(builder.Environment.WebRootPath, "images", "uploads");
    builder.Services.AddScoped<IImageHandler>(_ => new LocalImageHandler(localPath));
}



builder.Services.AddContexts(builder.Configuration.GetConnectionString("SqlConnection")!);
builder.Services.AddLocalIdentity(builder.Configuration);
builder.Services.AddRepositories(builder.Configuration);
builder.Services.AddServices(builder.Configuration);

var app = builder.Build();
app.UseHsts();
app.UseHttpsRedirection();

app.UseRewriter(new RewriteOptions().AddRedirect("^$", "/admin/overview"));
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseDefaultRoles();
app.UseDefaultAdminAccount();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Overview}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapHub<NotificationHub>("/notificationHub");

app.Run();
