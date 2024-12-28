using System.Text.Json.Serialization;
using UrlShortener.Application.Common.Interfaces;
using UrlShortener.Domain.Entities;
using UrlShortener.Domain.Events;

namespace UrlShortener.Application.Urls.Commands.ShortenUrl;

public record ShortenUrlCommand : IRequest<string>
{
    public string? Url { get; set; }

    [JsonIgnore]
    public string? Host { get; set; }
}

public class ShortenUrlCommandValidator : AbstractValidator<ShortenUrlCommand>
{
    public ShortenUrlCommandValidator()
    {
        RuleFor(x => x.Url)
            .Must(x => Uri.TryCreate(x, UriKind.Absolute, out _))
            .WithMessage("Invalid URL");

        RuleFor(x => x.Host)
            .Must(x => Uri.TryCreate(x, UriKind.Absolute, out _))
            .WithMessage("Invalid URL");
    }
}

public class ShortenUrlCommandHandler : IRequestHandler<ShortenUrlCommand, string>
{
    private readonly IApplicationDbContext _context;

    public ShortenUrlCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(ShortenUrlCommand request, CancellationToken cancellationToken)
    {
        var entity = new Url
        {
            OriginalUrl = request.Url,
            ShortenedUrl = Guid.NewGuid().ToString().Replace("-", string.Empty)[..9]
        };

        entity.AddDomainEvent(new UrlShortenedEvent(entity));

        _context.Urls.Add(entity);

        await _context.SaveChangesAsync(cancellationToken);

        return request.Host + "/api/urls/" + entity.ShortenedUrl;
    }
}
