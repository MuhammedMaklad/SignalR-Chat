using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SignalRChatApp.Contracts;
using SignalRChatApp.Data;
using SignalRChatApp.Hubs;
using SignalRChatApp.Models;
using SignalRChatApp.Services;

namespace SignalRChatApp
{
  public class Program
  {
    public static void Main(string[] args)
    {
      var builder = WebApplication.CreateBuilder(args);

      // Add services to the container.
      builder.Services.AddControllersWithViews();
      builder.Services.AddAntiforgery();

      var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=chat.db";
      builder.Services.AddDbContext<AppDbContext>(options =>
          options.UseSqlServer(connectionString));

      builder.Services.AddIdentity<AppUser, IdentityRole>()
      .AddEntityFrameworkStores<AppDbContext>();

      builder.Services.ConfigureApplicationCookie(options =>
      {
          options.Cookie.HttpOnly = true;
          options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
          options.Cookie.SameSite = SameSiteMode.Lax;
          options.LoginPath = "/Auth/Login";
          options.AccessDeniedPath = "/Auth/Login";
      });

      builder.Services.AddScoped<IAuthService, AuthService>();
      builder.Services.AddScoped<IChatService, ChatService>();
      builder.Services.AddScoped<IGroupService, GroupService>();
      builder.Services.AddSignalR();

      var app = builder.Build();

      if (!app.Environment.IsDevelopment())
      {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
      }

      app.UseHttpsRedirection();
      app.UseStaticFiles();

      app.UseRouting();

      app.UseAuthentication();
      app.UseAuthorization();

      app.UseAntiforgery();

      app.MapStaticAssets();
      app.MapHub<ChatHub>("/chatHub");
      app.MapControllerRoute(
          name: "default",
          pattern: "{controller=Home}/{action=Index}/{id?}")
          .WithStaticAssets();

      app.Run();
    }
  }
}
