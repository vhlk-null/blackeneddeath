using System.Linq.Expressions;
using BuildingBlocks.Messaging.Events;
using BuildingBlocks.Messaging.Events.Albums;
using BuildingBlocks.Repositories;
using FluentAssertions;
using MassTransit;
using Microsoft.Extensions.Logging;
using Moq;
using UserContent.Application.Consumers;
using UserContent.Domain.Models;
using UserContent.Infrastructure.Data;
using Xunit;

namespace UserContent.ApplicationTests.Consumers;

public class AlbumCreatedConsumerTests
{
    private readonly Mock<IRepository<UserContentContext>> _repoMock = new();
    private readonly AlbumCreatedConsumer _sut;

    public AlbumCreatedConsumerTests()
    {
        _repoMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _sut = new AlbumCreatedConsumer(_repoMock.Object, Mock.Of<ILogger<AlbumCreatedConsumer>>());
    }

    [Fact]
    public async Task Consume_NewAlbum_AddsWithBandIdsAndSaves()
    {
        Guid albumId = Guid.NewGuid();
        Guid bandId = Guid.NewGuid();
        AlbumCreatedIntegrationEvent message = new()
        {
            AlbumId = albumId,
            Title = "Symbolic",
            Slug = "symbolic-1995",
            ReleaseYear = 1995,
            Format = 1,
            Type = 1,
            Bands = [new AlbumBandInfo(bandId, "Death", "death")],
            Countries = []
        };

        Mock<ConsumeContext<AlbumCreatedIntegrationEvent>> ctx = new();
        ctx.Setup(x => x.Message).Returns(message);
        ctx.Setup(x => x.CancellationToken).Returns(CancellationToken.None);

        await _sut.Consume(ctx.Object);

        _repoMock.Verify(x => x.AddAsync(
            It.Is<Album>(a => a.Id == albumId && a.Title == "Symbolic" && a.BandIds!.Contains(bandId.ToString())),
            It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
