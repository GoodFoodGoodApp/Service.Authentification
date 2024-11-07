namespace API.Endpoints;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.User;
using Application.User.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Permissions = Permissions.Permissions;


public static class ProfileEndpoints
{
    public static void MapProfileEndpoints(this WebApplication app)
    {

        var root = app.MapGroup("/api/profile")
            .WithTags("profile")
            .WithDescription("Endpoints for user profile management")
            .WithOpenApi();

        _ = root.MapGet("/hello-world", GetHelloWorld)
            .WithDescription("A simple hello world endpoint")
            .WithTags("auth")
            .Produces<string>(StatusCodes.Status200OK)
            .RequireAuthorization();

        _ = root.MapGet("/token", GetToken)
            .Produces<string>(StatusCodes.Status200OK)
            .RequireAuthorization();

        _ = root.MapGet("/", GetUser)
            .WithDescription("Get the user name")
            .WithTags("profile")
            .Produces<string>(StatusCodes.Status200OK)
            .RequireAuthorization();

        _ = root.MapGet("/admin", () => "Hello admin")
            .Produces<string>(StatusCodes.Status200OK)
            .RequireAuthorization(root => root.RequireRole(Permissions.AdminRole));

        _ = root.MapGet("/user", () => "Hello user")
            .Produces<string>(StatusCodes.Status200OK)
            .RequireAuthorization(root => root.RequireRole(Permissions.ClientRole));

        _ = root.MapGet("/manager", () => "Hello manager")
            .Produces<string>(StatusCodes.Status200OK)
            .RequireAuthorization(root => root.RequireRole(Permissions.ManagerRole));

        _ = root.MapGet("/delivery", () => "Hello delivery")
            .Produces<string>(StatusCodes.Status200OK)
            .RequireAuthorization(root => root.RequireRole(Permissions.DeliveryRole));

    }

    public static IResult GetToken(ClaimsPrincipal user, [FromServices] IConfiguration configuration)
    {
        var token = GenerateJwtToken(user, configuration);
        return Results.Ok(token);
    }

    public static IResult GetUser(ClaimsPrincipal user)
    {
        return Results.Ok(user.Identity?.Name);
    }

    public static IResult GetHelloWorld()
    {
        try
        {
            return Results.Ok("Hello world! It seems that you are authorize to do this action.");
        }
        catch (Exception ex)
        {
            if (ex is UnauthorizedAccessException)
            {
                return Results.Problem("You must login to access!");
            }
            return Results.Problem(
                ex.StackTrace,
                $"An error occurred while fetching user data: {ex.Message}",
                StatusCodes.Status500InternalServerError
            );
        }
    }


    public static async Task<IResult> UpdateProfile(ClaimsPrincipal user, [FromBody] UserProfile profile, [FromServices] IUserService userService)
    {
        try
        {
            await userService.UpdateUserProfile(profile);
            return Results.NoContent();
        }
        catch (Exception ex)
        {
            return Results.Problem(
                ex.StackTrace,
                $"An error occurred while updating user data: {ex.Message}",
                StatusCodes.Status500InternalServerError
            );
        }
    }


    public static string GenerateJwtToken(ClaimsPrincipal user, IConfiguration configuration)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var key = Encoding.UTF8.GetBytes(configuration["Jwt:Key"]);


        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(
            [
            new(ClaimTypes.NameIdentifier, user.FindFirst(ClaimTypes.NameIdentifier).Value),
            new(ClaimTypes.Name, user.Identity.Name),
            new(ClaimTypes.Role, user.FindFirst(ClaimTypes.Role).Value),
            new(ClaimTypes.Email, user.FindFirst(ClaimTypes.Email).Value)

            ]),
            Expires = DateTime.UtcNow.AddHours(1),
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

}
