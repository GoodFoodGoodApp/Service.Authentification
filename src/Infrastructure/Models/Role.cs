namespace Infrastructure.Models;
using Microsoft.AspNetCore.Identity;

public class Role : IdentityRole<Guid>
{
    public Role(string roleName) : base(roleName)
    {
    }

    public Role()
    {

    }
}
