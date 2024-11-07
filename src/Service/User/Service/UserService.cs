namespace Application.User.Service;

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Application.User.Entities;
using Microsoft.AspNetCore.Http;

public class UserService : IUserService
{
    //TODO Review API layers
    public static readonly string JwtKey = "development_only_key_never_use_in_production_minimum_64_characters_long_for_security";
    public static readonly string Issuer = "AuthService";
    public static readonly string Audience = "Services";

    private readonly IUserRepository userRepository;

    private readonly IHttpContextAccessor httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
    {
        this.httpContextAccessor = httpContextAccessor;
        this.userRepository = userRepository;
    }
    public Task<string> GetName()
    {
        var result = this.GetClaimsIdentity().Identity?.Name ?? string.Empty;

        return Task.FromResult(result);
    }

    public async Task<UserProfile> GetUserProfile()
    {
        var userEmail = this.GetClaimsIdentity().Identity?.Name;

        if (string.IsNullOrEmpty(userEmail))
        {
            throw new TaskCanceledException();
        }

        return await this.userRepository.GetUserProfile(userEmail);
    }

    public Task UpdateUserProfile(UserProfile profile)
    {
        throw new NotImplementedException();
    }

    private ClaimsPrincipal GetClaimsIdentity()
    {
        if (this.httpContextAccessor.HttpContext != null)
        {
            return this.httpContextAccessor.HttpContext.User;
        }
        else
        {
            return null;
        }
    }

}
