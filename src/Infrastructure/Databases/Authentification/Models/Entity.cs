namespace AuthentificationApi.Infrastructure.Databases.Catalog.Models;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal abstract record Entity
{
    public Guid Id { get; init; }
    public DateTime DateCreated { get; init; }
    public DateTime DateModified { get; set; }
    public bool IsActive { get; set; }
}
