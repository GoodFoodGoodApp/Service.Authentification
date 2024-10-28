public static class UsersEndpoints
{
    public static WebApplication MapUserEndpoints(this WebApplication app)
    {
        var root = app.MapGroup("/api/user")
            .WithTags("user")
            .WithDescription("Get users")
            .WithOpenApi();



        return app;
    }

}