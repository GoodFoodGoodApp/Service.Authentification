namespace AuthentificationApi.Infrastructure.Databases.Catalog.Models;

using System.Diagnostics.CodeAnalysis;
using AuthentificationApi.Infrastructure.Databases.MoviesReviews.Models;

[ExcludeFromCodeCoverage]
internal record UserAccount : Entity
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string? Address { get; set; } // Nullable address field

    public UserLoginData LoginData { get; set; }  // One-to-one relationship with login data
    public UserRole Role { get; set; }  // One-to-one relationship with user role

}
