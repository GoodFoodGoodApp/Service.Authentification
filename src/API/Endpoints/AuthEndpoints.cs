namespace API.Endpoints;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Connections.Features;
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
            .WithOpenApi();

        _ = root.MapPost("/register", Register)
            .Produces<User>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Register a new user");

        _ = root.MapPost("/login", Login)
            .Produces<string>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Login with existing credentials");

        _ = root.MapPost("/refresh-token", RefreshToken)
            .Produces<string>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Refresh the access token using a refresh token");

        _ = root.MapGet("/me", GetMe)
            .RequireAuthorization()
            .Produces<string>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status401Unauthorized)
            .ProducesProblem(StatusCodes.Status500InternalServerError)
            .WithDescription("Get current user information");
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

    public static async Task<IResult> Register(UserBase request)
    {
        try
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            user.Username = request.Username;
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;

            return Results.Ok(user);
        }
        catch (Exception ex)
        {
            return Results.Problem(
                ex.StackTrace,
                $"An error occurred during registration: {ex.Message}",
                StatusCodes.Status500InternalServerError
            );
        }
    }

    public static async Task<IResult> Login(
        UserBase request,
        IConfiguration configuration,
        HttpResponse response)
    {
        try
        {
            if (user.Username != request.Username)
                return Results.BadRequest("User not found.");

            if (!VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt))
                return Results.BadRequest("Wrong password.");

            string token = CreateToken(user, configuration);
            var refreshToken = GenerateRefreshToken();
            SetRefreshToken(refreshToken, response);

            return Results.Ok(token);
        }
        catch (Exception ex)
        {
            return Results.Problem(
                ex.StackTrace,
                $"An error occurred during login: {ex.Message}",
                StatusCodes.Status500InternalServerError
            );
        }
    }

    public static async Task<IResult> RefreshToken(
        HttpRequest request,
        HttpResponse response,
        IConfiguration configuration)
    {
        try
        {
            var refreshToken = request.Cookies["refreshToken"];

            if (!user.RefreshToken.Equals(refreshToken))
                return Results.Unauthorized();

            if (user.TokenExpires < DateTime.Now)
                return Results.Unauthorized();

            string token = CreateToken(user, configuration);
            var newRefreshToken = GenerateRefreshToken();
            SetRefreshToken(newRefreshToken, response);

            return Results.Ok(token);
        }
        catch (Exception ex)
        {
            return Results.Problem(
                ex.StackTrace,
                $"An error occurred while refreshing token: {ex.Message}",
                StatusCodes.Status500InternalServerError
            );
        }
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

    #region Private Helper Methods
    private static RefreshToken GenerateRefreshToken()
    {
        var refreshToken = new RefreshToken
        {
            Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
            Expires = DateTime.Now.AddDays(7),
            Created = DateTime.Now
        };

        return refreshToken;
    }

    private static void SetRefreshToken(RefreshToken newRefreshToken, HttpResponse response)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Expires = newRefreshToken.Expires
        };
        response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);

        user.RefreshToken = newRefreshToken.Token;
        user.TokenCreated = newRefreshToken.Created;
        user.TokenExpires = newRefreshToken.Expires;
    }

    private static string CreateToken(UserBase user, IConfiguration configuration)
    {
        List<Claim> claims = new()
        {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, "Admin")
        };

        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            configuration.GetSection("AppSettings:Token").Value!));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
    }

    private static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }
    #endregion
}
