namespace Service.User;
using System.Threading.Tasks;

public interface IUserService
{
    //TODO cancellation Token ?
    //Task<IResult> Register(LoginRequest loginRequest);

    Task<string> GetName();

}
