using UrlShortener.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace UrlShortener.Infrastructure.Data.Configurations;

public class UrlConfiguration : IEntityTypeConfiguration<Url>
{
    public void Configure(EntityTypeBuilder<Url> builder)
    {
        builder.HasIndex(t => t.ShortenedUrl)
            .IsUnique();

        builder.Property(t => t.ShortenedUrl)
            .IsRequired();
    }
}
