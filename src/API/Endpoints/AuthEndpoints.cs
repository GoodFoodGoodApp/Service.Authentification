namespace API.Endpoints;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Connections.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.IdentityModel.Tokens;
using Service.Token;
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
    //public static IResult Register(LoginRequest loginRequest)
    //{
    //    try
    //    {
    //        if (loginRequest.Email.Contains("io"))
    //        {
    //            return Results.BadRequest("Invalid email");
    //        }
    //        Console.WriteLine(loginRequest.Email);
    //        Console.WriteLine(loginRequest.Password);

    //        return Results.Ok("User registered successfully");
    //    }
    //    catch (Exception ex)
    //    {
    //        return Results.Problem(ex.StackTrace, $"An error occured : {ex.Message}", StatusCodes.Status500InternalServerError);
    //    }
    //}


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
