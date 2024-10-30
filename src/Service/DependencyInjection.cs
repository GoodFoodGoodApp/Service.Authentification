namespace Service;

using Microsoft.Extensions.DependencyInjection;
using Service.User;
using Service.User.Service;

public static class DependencyInjection
{

    public static IServiceCollection AddService(this IServiceCollection services)
    {
        _ = services.AddHttpContextAccessor();
        _ = services.AddScoped<IUserService, UserService>();


        return services;
    }
}
