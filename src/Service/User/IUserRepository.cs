namespace Application.User;
using System.Threading.Tasks;
using Application.User.Entities;

public interface IUserRepository
{
    public Task<UserProfile> GetUserProfile(string Email);

    //public Task UpdateUserProfile(UserProfile profile);
}
