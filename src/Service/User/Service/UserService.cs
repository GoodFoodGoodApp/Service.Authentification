namespace Application.User.Service;

using System;
using System.Threading.Tasks;
using Application.User.Entities;
using Microsoft.AspNetCore.Http;

public class UserService : IUserService
{
    private readonly IUserRepository userRepository;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor, IUserRepository userRepository)
    {
        _httpContextAccessor = httpContextAccessor;
        this.userRepository = userRepository;
    }
    public Task<string> GetName()
    {


        var result = string.Empty;
        if (this._httpContextAccessor.HttpContext != null)
        {
            result = this._httpContextAccessor.HttpContext.User.Identity?.Name ?? string.Empty;
        }
        return Task.FromResult(result);
    }

    public async Task<UserProfile> GetUserProfile()
    {
        var userEmail = this._httpContextAccessor.HttpContext.User.Identity?.Name;
        if(userEmail == string.Empty)
        {
            throw new Exception("User not found");
        }
        return await this.userRepository.GetUserProfile(userEmail);
    }
}
