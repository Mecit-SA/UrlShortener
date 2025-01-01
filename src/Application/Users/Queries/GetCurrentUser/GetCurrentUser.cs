using UrlShortener.Application.Common.Interfaces;

namespace UrlShortener.Application.Users.Queries.GetCurrentUser;

public record GetCurrentUserQuery : IRequest<GetCurrentUserResponse>
{
}

public class GetCurrentUserQueryValidator : AbstractValidator<GetCurrentUserQuery>
{
    public GetCurrentUserQueryValidator()
    {
    }
}

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, GetCurrentUserResponse>
{
    private readonly IUser _user;

    public GetCurrentUserQueryHandler(IUser user)
    {
        _user = user;
    }

    public Task<GetCurrentUserResponse> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var response = !_user.IsLoggedIn
            ? new GetCurrentUserResponse()
            : new GetCurrentUserResponse
            {
                IsLoggedIn = _user.IsLoggedIn,
                User = new GetCurrentUserDTO
                {
                    Username = _user.Username,
                }
            };

        return Task.FromResult(response);
    }
}

public class GetCurrentUserResponse
{
    public bool IsLoggedIn { get; set; }

    public GetCurrentUserDTO? User { get; set; }
}

public class GetCurrentUserDTO
{
    public string? Username { get; set; }
}
