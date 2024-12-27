namespace UrlShortener.Domain.Entities;

public class Url : BaseAuditableEntity
{
    public string? OriginalUrl { get; set; }

    public string? ShortenedUrl { get; set; }
}
