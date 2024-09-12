namespace AuthentificationApi.Infrastructure.Databases.Catalog.Models;

using System.Diagnostics.CodeAnalysis;
using AuthentificationApi.Infrastructure.Databases.MoviesReviews.Models;

[ExcludeFromCodeCoverage]
internal record UserRole : Entity
{
    public int Id { get; set; }

    // Name of the role (e.g., "Admin", "User").
    public string Name { get; set; }

    // Description of the role.
    public string? Description { get; set; }

    // Link to the associated user account.
    public ICollection<UserAccount> UserAccounts { get; set; } = new List<UserAccount>();  // Many-to-many relationship with UserAccount
}
