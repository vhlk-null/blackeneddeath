using FluentAssertions;
using Library.Domain.Enums;
using Library.Domain.Models;
using Library.Domain.ValueObjects;
using Library.Domain.ValueObjects.Ids;
using Library.Infrastructure.Data;
using Library.Infrastructure.Data.Interceptors;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Library.InfrastructureTests.Interceptors;

public class AuditableEntityInterceptorTests
{
    private static LibraryContext CreateContext()
    {
        DbContextOptions<LibraryContext> options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        return new LibraryContext(options);
    }

    [Fact]
    public void UpdateEntities_NullContext_DoesNotThrow()
    {
        AuditableEntityInterceptor interceptor = new AuditableEntityInterceptor();

        Action act = () => interceptor.UpdateEntities(null);

        act.Should().NotThrow();
    }

    [Fact]
    public void UpdateEntities_AddedEntity_SetsCreatedAndModifiedFields()
    {
        AuditableEntityInterceptor interceptor = new AuditableEntityInterceptor();
        using LibraryContext context = CreateContext();

        Genre genre = Genre.Create(GenreId.Of(Guid.NewGuid()), "Death Metal", null);
        context.Genres.Add(genre);

        interceptor.UpdateEntities(context);

        genre.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        genre.CreatedBy.Should().NotBeNullOrEmpty();
        genre.LastModifiedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        genre.LastModifiedBy.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public void UpdateEntities_ModifiedEntity_SetsOnlyModifiedFields()
    {
        AuditableEntityInterceptor interceptor = new AuditableEntityInterceptor();
        using LibraryContext context = CreateContext();

        Genre genre = Genre.Create(GenreId.Of(Guid.NewGuid()), "Death Metal", null);
        context.Genres.Add(genre);
        context.SaveChanges();

        genre.Update("Black Metal", null);
        interceptor.UpdateEntities(context);

        genre.LastModifiedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        genre.LastModifiedBy.Should().NotBeNullOrEmpty();
    }
}
