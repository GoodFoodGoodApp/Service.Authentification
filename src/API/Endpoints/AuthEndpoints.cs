namespace API.Endpoints;
using Microsoft.AspNetCore.Identity;
using Service.User;
using Service.User.DTO;

public static class AuthEndpoints
{
    public static User user = new User();

    public static WebApplication MapAuthEndpoints(this WebApplication app)
    {

        var root = app.MapGroup("/api/auth")
            .WithTags("auth")
            .WithDescription("Endpoints for authentication and authorization")
            .WithOpenApi()
            .MapIdentityApi<IdentityUser>();

        return app;
    }

    public static async Task<IResult> GetMe(IUserService userService)
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
