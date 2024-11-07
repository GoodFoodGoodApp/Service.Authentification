using API.Extensions;
using Serilog;

var builder = WebApplication
    .CreateBuilder(args)
    .ConfigureApplicationBuilder();

var app = builder
    .Build()
    .ConfigureApplicationAsync();

try
{
    Log.Information("Starting host");
    app.Result.Run();
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
