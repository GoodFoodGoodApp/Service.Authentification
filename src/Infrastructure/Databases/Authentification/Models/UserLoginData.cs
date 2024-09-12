namespace AuthentificationApi.Infrastructure.Databases.Catalog.Models;

using System.Diagnostics.CodeAnalysis;
using AuthentificationApi.Infrastructure.Databases.Catalog.Models;

[ExcludeFromCodeCoverage]
internal record UserLoginData : Entity
{
    public int Id { get; set; }

    // The hashed version of the user's password.
    public string PasswordHash { get; set; }

    // Salt used for password hashing.
    public string PasswordSalt { get; set; }

    // Token for email confirmation or password reset.
    public string? EmailConfirmationToken { get; set; }

    // Expiration time for the email confirmation token.
    public DateTime? EmailConfirmationTokenExpiration { get; set; }

    // Token for password recovery.
    public string? PasswordRecoveryToken { get; set; }

    // Expiration time for the password recovery token.
    public DateTime? PasswordRecoveryTokenExpiration { get; set; }

    // Link to the associated user account.
    public Guid UserAccountId { get; set; }
    public UserAccount UserAccount { get; set; } // One-to-one relationship with UserAccount
}
