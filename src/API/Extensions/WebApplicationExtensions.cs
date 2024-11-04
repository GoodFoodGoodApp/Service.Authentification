namespace API.Extensions;

using API.Endpoints;
using Infrastructure.Data;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Serilog;

public static class WebApplicationExtensions
{
    public static async Task<WebApplication> ConfigureApplicationAsync(this WebApplication app)
    {
        #region Logging

        _ = app.UseSerilogRequestLogging();

        #endregion Logging

        #region Security

        _ = app.UseHsts();

        #endregion Security

        #region API Configuration

        _ = app.UseHttpsRedirection();
        _ = app.UseAuthentication();
        _ = app.UseAuthorization();

        #endregion API Configuration

        #region ConfigureSwagger
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        #endregion

        #region ConfigureEndpoints
        // Register endpoints
        app.MapAuthEndpoints();
        app.MapProfileEndpoints();
        #endregion

        #region SeedingData
        using (var scope = app.Services.CreateScope())
        {
            var services = scope.ServiceProvider;
            try
            {
                var context = services.GetRequiredService<DataContext>();
                var roleManager = services.GetRequiredService<RoleManager<Role>>();
                var userManager = services.GetRequiredService<UserManager<User>>();

                await SeedRolesAsync(roleManager);
                await SeedAdminUserAsync(userManager);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "An error occurred while seeding the database.");
            }
        }
        #endregion

        return app;
    }

    private static async Task SeedRolesAsync(RoleManager<Role> roleManager)
    {
        var roles = new[] { "admin", "manager", "delivery", "user" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new Role(role));
            }
        }
    }

    private static async Task SeedAdminUserAsync(UserManager<User> userManager)
    {
        var email = "admin@gmail.com";
        var password = "String1!";

        if (await userManager.FindByEmailAsync(email) == null)
        {
            var adminUser = new User
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(adminUser, password);
            await userManager.AddToRoleAsync(adminUser, "admin");
        }
    }
}
