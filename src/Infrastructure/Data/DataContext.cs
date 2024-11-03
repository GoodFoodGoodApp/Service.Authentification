namespace Infrastructure.Data;

using System.Reflection.Emit;
using Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

public class DataContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
{
    public override DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        //modelBuilder.ApplyConfiguration();

        // Data seeding


    }


}
