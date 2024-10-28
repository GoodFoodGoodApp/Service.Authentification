namespace Infra.Services
{
    public interface IAuthService
    {
        Task<string> AuthenticateAsync(string username, string password);
        Task<bool> ValidateTokenAsync(string token);
        Task<IEnumerable<string>> GetUserPermissionsAsync(Guid userId);
    }
}