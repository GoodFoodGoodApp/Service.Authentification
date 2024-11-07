namespace Application.User;
using System.Threading.Tasks;
using Application.User.Entities;

public interface IUserService
{

    Task<string> GetName();

    Task<UserProfile> GetUserProfile();

    Task UpdateUserProfile(UserProfile profile);

}
