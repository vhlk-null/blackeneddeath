using System.Linq.Expressions;
using BuildingBlocks.Messaging.Events.Albums;
using BuildingBlocks.Repositories;
using FluentAssertions;
using MassTransit;
using Moq;
using UserContent.Application.Consumers;
using UserContent.Domain.Models;
using UserContent.Infrastructure.Data;
using Xunit;

namespace UserContent.ApplicationTests.Consumers;

public class AlbumUpdatedConsumerTests
{
    private readonly Mock<IRepository<UserContentContext>> _repoMock = new();
    private readonly AlbumUpdatedConsumer _sut;

    public AlbumUpdatedConsumerTests()
    {
        _repoMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _sut = new AlbumUpdatedConsumer(_repoMock.Object);
    }

    private static AlbumUpdatedIntegrationEvent BuildMessage(Guid albumId, string title = "Symbolic") => new()
    {
        AlbumId = albumId, Title = title, ReleaseDate = 1995, Format = 1, Type = 1, Bands = [], Countries = []
    };

    private static ConsumeContext<AlbumUpdatedIntegrationEvent> MockContext(AlbumUpdatedIntegrationEvent msg)
    {
        Mock<ConsumeContext<AlbumUpdatedIntegrationEvent>> ctx = new();
        ctx.Setup(x => x.Message).Returns(msg);
        ctx.Setup(x => x.CancellationToken).Returns(CancellationToken.None);
        return ctx.Object;
    }

    [Fact]
    public async Task Consume_ExistingAlbum_UpdatesFields()
    {
        Guid albumId = Guid.NewGuid();
        Album album = new() { Id = albumId, Title = "OldTitle" };
        _repoMock.Setup(x => x.GetByAsync<Album>(It.IsAny<Expression<Func<Album, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(album);

        await _sut.Consume(MockContext(BuildMessage(albumId, "Symbolic")));

        album.Title.Should().Be("Symbolic");
        _repoMock.Verify(x => x.Update(album), Times.Once);
        _repoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Consume_AlbumNotFound_CreatesNewAlbum()
    {
        _repoMock.Setup(x => x.GetByAsync<Album>(It.IsAny<Expression<Func<Album, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Album?)null);

        Guid albumId = Guid.NewGuid();
        await _sut.Consume(MockContext(BuildMessage(albumId)));

        _repoMock.Verify(x => x.AddAsync(It.Is<Album>(a => a.Id == albumId), It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
