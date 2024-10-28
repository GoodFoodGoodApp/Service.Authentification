using Infra.Services;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace API.Endpoints
{
    public static class AuthEndpoints
    {

        public static WebApplication MapAuthEndpoints(this WebApplication app) {
            var root = app.MapGroup("/api/auth")
                    .WithTags("user")
                    .WithDescription("Login and authorisation retrieval")
                    .WithOpenApi();

            _ = app.MapPost("/login", Authenticate)
                .Produces<IResult>()
                .ProducesProblem(StatusCodes.Status500InternalServerError);

            _ = app.MapGet("/validate", Validate)
                .Produces<IResult>()
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .ProducesProblem(StatusCodes.Status500InternalServerError);

            return app;
        }


    public static async Task<IResult> Authenticate (LoginRequest request, [FromServices] IAuthService authService)
    {
        try
        {
                var token = await authService.AuthenticateAsync(request.Email , request.Password);

                return Results.Ok(new { token });
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }


    public static async Task<IResult> Validate (HttpContext context, [FromServices] IAuthService authService)
    {
        try
        {
            var authorization = context.Request.Headers.Authorization.ToString();

            if(string.IsNullOrEmpty(authorization) || !authorization.StartsWith("Bearer "))
            {
                return Results.BadRequest("Invalid token format");
            }
            var token = authorization.Substring("Bearer ".Length);
            var isValid = await authService.ValidateTokenAsync(token);

            return isValid ? Results.Ok() : Results.Unauthorized();
        }
        catch (Exception ex)
        {

            return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
        }
    }

    public static async Task<IResult> GetPermissions([FromRoute] Guid userId, [FromServices] AuthService authService)
        {
            try
            {
                var permissions = await authService.GetUserPermissionsAsync(userId);
                return Results.Ok(permissions);
            }
            catch (Exception ex)
            {

                return Results.Problem(ex.StackTrace, ex.Message, StatusCodes.Status500InternalServerError);
            }
        }

}
}
