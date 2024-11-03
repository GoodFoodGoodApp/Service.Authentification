namespace API.Endpoints;

using Application.User;
using Infrastructure.Models;
//using Service.User.DTO = Dto ;

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
