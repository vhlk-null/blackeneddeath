using System.Linq.Expressions;
using BuildingBlocks.Messaging.Events.Bands;
using BuildingBlocks.Repositories;
using FluentAssertions;
using MassTransit;
using Moq;
using UserContent.Application.Consumers;
using UserContent.Domain.Models;
using UserContent.Infrastructure.Data;
using Xunit;

namespace UserContent.ApplicationTests.Consumers;

public class BandRemovedConsumerTests
{
    private readonly Mock<IRepository<UserContentContext>> _repoMock = new();
    private readonly BandRemovedConsumer _sut;

    public BandRemovedConsumerTests()
    {
        _repoMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _sut = new BandRemovedConsumer(_repoMock.Object);
    }

    private static ConsumeContext<BandRemovedIntegrationEvent> MockContext(Guid bandId)
    {
        Mock<ConsumeContext<BandRemovedIntegrationEvent>> ctx = new();
        ctx.Setup(x => x.Message).Returns(new BandRemovedIntegrationEvent { BandId = bandId, Name = "Death" });
        ctx.Setup(x => x.CancellationToken).Returns(CancellationToken.None);
        return ctx.Object;
    }

    [Fact]
    public async Task Consume_ExistingBand_DeletesAndSaves()
    {
        Guid bandId = Guid.NewGuid();
        Band band = new() { BandId = bandId, BandName = "Death" };
        _repoMock.Setup(x => x.GetByAsync<Band>(It.IsAny<Expression<Func<Band, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(band);

        await _sut.Consume(MockContext(bandId));

        _repoMock.Verify(x => x.Delete(band), Times.Once);
        _repoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Consume_BandNotFound_DoesNothing()
    {
        _repoMock.Setup(x => x.GetByAsync<Band>(It.IsAny<Expression<Func<Band, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Band?)null);

        await _sut.Consume(MockContext(Guid.NewGuid()));

        _repoMock.Verify(x => x.Delete(It.IsAny<Band>()), Times.Never);
        _repoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
