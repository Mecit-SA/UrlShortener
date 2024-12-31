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
        var query = _context.Urls.Where(l => l.ShortenedUrl == request.ShortenedUrl);

        var entity = await query.SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.ShortenedUrl!, entity);

        await query.ExecuteUpdateAsync(update => 
            update.SetProperty(u => u.VisitCount, u => u.VisitCount + 1), 
            cancellationToken);

        return entity.OriginalUrl!;
    }
}
