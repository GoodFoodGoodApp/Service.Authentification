namespace API.Endpoints;

using Application.User;
using Application.User.Entities;
using Microsoft.AspNetCore.Mvc;

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

        _ = root.MapGet("/me", GetMe)
               .WithDescription("Get current user information")
               .WithTags("auth")
               .Produces<string>(StatusCodes.Status200OK)
               .Produces(StatusCodes.Status401Unauthorized)
               .RequireAuthorization();

        _ = root.MapGet("", GetProfile)
            .WithDescription("Get the profile of the user")
            .WithTags("profile")
            .Produces<UserProfile>(StatusCodes.Status200OK)
            .RequireAuthorization();
    }

    public static async Task<IResult> GetProfile([FromServices] IUserService userService)
    {
        try
        {
            var profile = await userService.GetUserProfile();
            return Results.Ok(profile);
        }
        catch (Exception ex)
        {
            return Results.Problem(
                ex.StackTrace,
                $"An error occurred while fetching user data: {ex.Message}",
                StatusCodes.Status500InternalServerError
            );
        }
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

    public static IResult GetMe(IUserService userService)
    {
        try
        {
            var userName = userService.GetName();
            return Results.Ok(userName);
        }
        catch (Exception ex)
        {
            return Results.Problem(
                ex.StackTrace,
                $"An error occurred while fetching user data: {ex.Message}",
                StatusCodes.Status500InternalServerError
            );
        }
    }

}
