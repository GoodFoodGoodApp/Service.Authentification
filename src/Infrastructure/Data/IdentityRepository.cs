namespace Infrastructure.Data;
using System.Threading.Tasks;
using Application.User;
using Application.User.Entities;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

internal class IdentityRepository : IUserRepository
{
    private readonly DataContext context;
    private readonly IMapper mapper;

    public IdentityRepository(DataContext context,
                              IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }
    public virtual async Task<UserProfile> GetUserProfile(string email)
    {
        var user = await this.context.Users.FirstAsync(u => u.Email == email);

        return this.mapper.Map<UserProfile>(user);
    }
}
