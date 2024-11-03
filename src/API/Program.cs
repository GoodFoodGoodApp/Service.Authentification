using System.Text;
using API.Endpoints;
using Infrastructure;
using Infrastructure.Data;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Service;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

#region ConfigureBuilderApplication

// Add services to the container.
builder.Services.AddInfrastructure(builder.Environment);

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = "Identity.Bearer";
//    options.DefaultChallengeScheme = "Identity.Bearer";
//})
//.AddJwtBearer("Identity.Bearer", options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = builder.Configuration["Jwt:Issuer"],
//        ValidAudience = builder.Configuration["Jwt:Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//    };
//});


builder.Services.AddIdentityApiEndpoints<User>()
    .AddRoles<Role>()
    .AddEntityFrameworkStores<DataContext>()
    .AddRoleManager<RoleManager<Role>>()
    .AddUserManager<UserManager<User>>()
    .AddSignInManager<SignInManager<User>>()
    .AddTokenProvider<DataProtectorTokenProvider<User>>(IdentityConstants.BearerScheme);

builder.Services.AddAuthentication();

builder.Services.AddService();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Description = "Standard Authorization header using the Bearer scheme (\"Bearer {token}\")",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthorization();

var app = builder.Build();
#endregion ConfigureBuilderApplication

#region ConfigureApplication

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

//Register endpoints
app.MapAuthEndpoints();
app.MapProfileEndpoints();

// Seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DataContext>();
    var roleManager = services.GetRequiredService<RoleManager<Role>>();

    var roles = new[] { "admin", "manager", "delivery", "user" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new Role(role));
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<User>>();

    var email = "admin@gmail.com";
    var password = "String1!";

    if (await userManager.FindByEmailAsync(email) == null)
    {
        var adminUser = new User
        {
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };
        await userManager.CreateAsync(adminUser, password);
        await userManager.AddToRoleAsync(adminUser, "admin");
    }
}

#endregion ConfigureApplication

app.Run();
