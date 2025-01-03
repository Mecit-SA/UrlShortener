using Microsoft.Extensions.Caching.Distributed;
using UrlShortener.Application.Common.Interfaces;
using UrlShortener.Domain.Entities;

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
    private readonly IDistributedCache _distributedCache;

    public RedirectToUrlQueryHandler(IApplicationDbContext context, IDistributedCache distributedCache)
    {
        _context = context;
        _distributedCache = distributedCache;
    }

    public async Task<string> Handle(GetOriginalUrlQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Urls.Where(l => l.ShortenedUrl == request.ShortenedUrl);

        var cachedUrl = await _distributedCache.GetStringAsync(request.ShortenedUrl!, cancellationToken);
        if(cachedUrl != null)
        {
            await IncreaseVisitCountAsync(query, cancellationToken);
            return cachedUrl;
        }

        var entity = await query.SingleOrDefaultAsync(cancellationToken);

        Guard.Against.NotFound(request.ShortenedUrl!, entity);

        await IncreaseVisitCountAsync(query, cancellationToken);
        
        await _distributedCache.SetStringAsync(entity.ShortenedUrl!, entity.OriginalUrl!, cancellationToken);

        return entity.OriginalUrl!;
    }

    private static async Task IncreaseVisitCountAsync(IQueryable<Url> query, CancellationToken cancellationToken)
    {
        await query.ExecuteUpdateAsync(update =>
            update.SetProperty(u => u.VisitCount, u => u.VisitCount + 1),
            cancellationToken);
    }
}
