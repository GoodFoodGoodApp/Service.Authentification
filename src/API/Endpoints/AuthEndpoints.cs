namespace API.Endpoints;

using Infrastructure.Models;

public static class AuthEndpoints
{
    public static WebApplication MapAuthEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/auth")
            .WithTags("auth")
            .WithDescription("Endpoints for authentication and authorization")
            .WithOpenApi()
            .MapIdentityApi<User>();

        return app;
    }

}
