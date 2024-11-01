namespace Infrastructure;

using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IHostEnvironment hostEnvironment)
    {

        _ = services.AddDbContext<DataContext>(options => options.UseSqlite("Data Source=../Infrastructure/data/app.db"));

        // Build the service provider to get the DataContext
        var serviceProvider = services.BuildServiceProvider();
        var context = serviceProvider.GetService<DataContext>();

        if (hostEnvironment.IsDevelopment())
        {
            // Ensure database is created and all migrations are applied
            context.Database.EnsureDeleted();   // Optional: Clear existing database
            context.Database.EnsureCreated();
        }
        else
        {
            if (context != null)
            {
                // Check if any pending migrations
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }
            }

        }
        return services;
    }
}

