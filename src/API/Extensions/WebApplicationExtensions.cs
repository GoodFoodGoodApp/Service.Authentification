namespace API.Extensions;

using System.Security.Claims;
using API.Endpoints;
using Infrastructure.Data;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Serilog;
using Permissions = Permissions.Permissions;

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
        if (app.Environment.IsDevelopment())
        {
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
                    await SeedClientUserAsync(userManager);
                    await SeedDeliveryUserAsync(userManager);
                    await SeedManagerUserAsync(userManager);
                }
                catch (Exception ex)
                {
                    Log.Logger.Error(ex, "An error occurred while seeding the database.");
                }
            }
        }
        #endregion

        return app;
    }

    private static async Task SeedRolesAsync(RoleManager<Role> roleManager)
    {
        var roles = new[] { "admin", "manager", "delivery", "user" };
        foreach (var role in Permissions.AllRoles)
        {
            {
                await roleManager.CreateAsync(new Role(role));

                var roleResult = await roleManager.FindByNameAsync(role);

                if (roleResult.Name == Permissions.AdminRole)
                {
                    foreach (var claim in Permissions.Admin.AllPermissions)
                    {
                        var roleClaim = new IdentityRoleClaim<Guid>
                        {
                            RoleId = roleResult.Id,
                            ClaimType = "permission",
                            ClaimValue = claim
                        };

                        await roleManager.AddClaimAsync(roleResult, new Claim(roleClaim.ClaimType, roleClaim.ClaimValue));
                    }
                }
                if (roleResult.Name == Permissions.ManagerRole)
                {
                    foreach (var claim in Permissions.Manager.AllPermissions)
                    {
                        var roleClaim = new IdentityRoleClaim<Guid>
                        {
                            RoleId = roleResult.Id,
                            ClaimType = "permission",
                            ClaimValue = claim
                        };

                        await roleManager.AddClaimAsync(roleResult, new Claim(roleClaim.ClaimType, roleClaim.ClaimValue));
                    }
                }
                if (roleResult.Name == Permissions.DeliveryRole)
                {
                    foreach (var claim in Permissions.Delivery.AllPermissions)
                    {
                        var roleClaim = new IdentityRoleClaim<Guid>
                        {
                            RoleId = roleResult.Id,
                            ClaimType = "permission",
                            ClaimValue = claim
                        };

                        await roleManager.AddClaimAsync(roleResult, new Claim(roleClaim.ClaimType, roleClaim.ClaimValue));
                    }
                }
                if (roleResult.Name == Permissions.ClientRole)
                {
                    foreach (var claim in Permissions.Client.AllPermissions)
                    {
                        var roleClaim = new IdentityRoleClaim<Guid>
                        {
                            RoleId = roleResult.Id,
                            ClaimType = "permission",
                            ClaimValue = claim
                        };

                        await roleManager.AddClaimAsync(roleResult, new Claim(roleClaim.ClaimType, roleClaim.ClaimValue));
                    }
                }
            }
        }
    }

    private static async Task SeedAdminUserAsync(UserManager<User> userManager)
    {
        var email = "admin@gmail.com";
        var password = "String1!";

        if (await userManager.FindByEmailAsync(email) == null)
        {
            var user = new User
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, Permissions.AdminRole);
        }
    }

    private static async Task SeedClientUserAsync(UserManager<User> userManager)
    {
        var email = "client@gmail.com";
        var password = "String1!";

        if (await userManager.FindByEmailAsync(email) == null)
        {
            var user = new User
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, Permissions.ClientRole);
        }
    }

    private static async Task SeedManagerUserAsync(UserManager<User> userManager)
    {
        var email = "manager@gmail.com";
        var password = "String1!";

        if (await userManager.FindByEmailAsync(email) == null)
        {
            var user = new User
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, Permissions.ManagerRole);
        }
    }

    private static async Task SeedDeliveryUserAsync(UserManager<User> userManager)
    {
        var email = "delivery@gmail.com";
        var password = "String1!";

        if (await userManager.FindByEmailAsync(email) == null)
        {
            var user = new User
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };
            await userManager.CreateAsync(user, password);
            await userManager.AddToRoleAsync(user, Permissions.DeliveryRole);
        }
    }


}   
