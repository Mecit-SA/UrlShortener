namespace UrlShortener.Domain.Events;

public class UrlShortenedEvent : BaseEvent
{
    public UrlShortenedEvent(Url url)
    {
        Url = url;
    }

    public Url Url { get; }
}
