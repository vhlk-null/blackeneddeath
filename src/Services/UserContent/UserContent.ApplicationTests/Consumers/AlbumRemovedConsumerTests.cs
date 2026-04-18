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

public class AlbumRemovedConsumerTests
{
    private readonly Mock<IRepository<UserContentContext>> _repoMock = new();
    private readonly AlbumRemovedConsumer _sut;

    public AlbumRemovedConsumerTests()
    {
        _repoMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _sut = new AlbumRemovedConsumer(_repoMock.Object);
    }

    private static ConsumeContext<AlbumRemovedIntegrationEvent> MockContext(Guid albumId)
    {
        Mock<ConsumeContext<AlbumRemovedIntegrationEvent>> ctx = new();
        ctx.Setup(x => x.Message).Returns(new AlbumRemovedIntegrationEvent { AlbumId = albumId, Title = "Symbolic" });
        ctx.Setup(x => x.CancellationToken).Returns(CancellationToken.None);
        return ctx.Object;
    }

    [Fact]
    public async Task Consume_ExistingAlbum_DeletesAndSaves()
    {
        Guid albumId = Guid.NewGuid();
        Album album = new() { Id = albumId, Title = "Symbolic" };
        _repoMock.Setup(x => x.GetByAsync<Album>(It.IsAny<Expression<Func<Album, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(album);

        await _sut.Consume(MockContext(albumId));

        _repoMock.Verify(x => x.Delete(album), Times.Once);
        _repoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Consume_AlbumNotFound_DoesNothing()
    {
        _repoMock.Setup(x => x.GetByAsync<Album>(It.IsAny<Expression<Func<Album, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Album?)null);

        await _sut.Consume(MockContext(Guid.NewGuid()));

        _repoMock.Verify(x => x.Delete(It.IsAny<Album>()), Times.Never);
        _repoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
