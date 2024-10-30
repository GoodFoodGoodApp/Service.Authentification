namespace Service.User.Service;

using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class UserService : IUserService
{

    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
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
}
