using UrlShortener.Domain.Entities;

namespace UrlShortener.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<TodoList> TodoLists { get; }

    DbSet<TodoItem> TodoItems { get; }

    DbSet<Url> Urls { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
