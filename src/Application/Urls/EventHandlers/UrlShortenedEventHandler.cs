using UrlShortener.Domain.Events;
using Microsoft.Extensions.Logging;

namespace UrlShortener.Application.TodoItems.EventHandlers;

public class UrlShortenedEventHandler : INotificationHandler<UrlShortenedEvent>
{
    private readonly ILogger<UrlShortenedEventHandler> _logger;

    public UrlShortenedEventHandler(ILogger<UrlShortenedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(UrlShortenedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UrlShortener Domain Event: {DomainEvent}", notification.GetType().Name);

        return Task.CompletedTask;
    }
}
