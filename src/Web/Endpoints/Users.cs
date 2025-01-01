using Microsoft.AspNetCore.Http.HttpResults;
using UrlShortener.Application.Users.Queries.GetCurrentUser;

namespace UrlShortener.Web.Endpoints;

public class Users : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
           .MapGet(GetCurrentUser);
    }

    public async Task<Ok<GetCurrentUserResponse>> GetCurrentUser(ISender sender)
    {
        var response = await sender.Send(new GetCurrentUserQuery());

        return TypedResults.Ok(response);
    }
}
