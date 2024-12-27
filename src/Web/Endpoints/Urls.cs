using Microsoft.AspNetCore.Http.HttpResults;
using UrlShortener.Application.Urls.Queries.GetOriginalUrl;
using UrlShortener.Application.Urls.Commands.ShortenUrl;

namespace UrlShortener.Web.Endpoints;

public class Urls : EndpointGroupBase
{
    public override void Map(WebApplication app)
    {
        app.MapGroup(this)
            .MapGet(GetOriginalUrl, "{shortenedUrl}")
            .MapPost(ShortenUrl);
    }

    public async Task<Ok<string>> GetOriginalUrl(ISender sender, string shortenedUrl)
    {
        var query = new GetOriginalUrlQuery { ShortenedUrl = shortenedUrl };

        var result = await sender.Send(query);

        return TypedResults.Ok(result);
    }

    public async Task<Ok<string>> ShortenUrl(ISender sender, IHttpContextAccessor httpContextAccessor, ShortenUrlCommand command)
    {
        var scheme = httpContextAccessor.HttpContext!.Request.Scheme;  // "http" or "https"
        var host = httpContextAccessor.HttpContext!.Request.Host.Value;  // "localhost:1234" or the actual domain
        command.Host = $"{scheme}://{host}";  // Combine the scheme and host

        var result = await sender.Send(command);

        return TypedResults.Ok(result);
    }
}
