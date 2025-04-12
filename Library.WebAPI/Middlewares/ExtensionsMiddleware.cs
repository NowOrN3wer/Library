using Library.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Library.WebAPI.Middlewares;

public static class ExtensionsMiddleware
{
    public static void CreateFirstUser(WebApplication app)
    {
        using var scoped = app.Services.CreateScope();
        var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

        if (userManager.Users.Any(p => p.UserName == "admin")) return;
        AppUser user = new()
        {
            UserName = "admin",
            Email = "admin@admin.com",
            FirstName = "admin",
            LastName = "admin",
            EmailConfirmed = true
        };

        userManager.CreateAsync(user, "1").Wait();
    }
}