namespace AuthentificationApi.Application.Movies.Entities;

using Application.Reviews.Entities;

public record Movie(Guid Id, string Title, ICollection<Review> Reviews = null);
