namespace UrlShortener.Application.Common.Interfaces;

public interface IUser
{
    bool IsLoggedIn { get; }
    string? Id { get; }
    string? Username { get; }
}
