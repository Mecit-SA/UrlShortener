using UrlShortener.Application.Common.Interfaces;

namespace UrlShortener.Application.Urls.Queries.GetOriginalUrl;

public record GetOriginalUrlQuery : IRequest<string>
{
    public string? ShortenedUrl { get; set; }
}

public class GetOriginalUrlQueryValidator : AbstractValidator<GetOriginalUrlQuery>
{
    public GetOriginalUrlQueryValidator()
    {
        RuleFor(x => x.ShortenedUrl)
            .NotEmpty()
            .WithMessage("The URL cannot be empty")
            .Matches(@"^\S+$")
            .WithMessage("The URL cannot contain spaces");
    }
}

public class RedirectToUrlQueryHandler : IRequestHandler<GetOriginalUrlQuery, string>
{
    private readonly IApplicationDbContext _context;

    public RedirectToUrlQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<string> Handle(GetOriginalUrlQuery request, CancellationToken cancellationToken)
    {
        var entity = await _context.Urls
            .Where(l => l.ShortenedUrl == request.ShortenedUrl)
            .SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.ShortenedUrl!, entity);

        return entity.OriginalUrl!;
    }
}
