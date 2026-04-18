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

public class BandUpdatedConsumerTests
{
    private readonly Mock<IRepository<UserContentContext>> _repoMock = new();
    private readonly BandUpdatedConsumer _sut;

    public BandUpdatedConsumerTests()
    {
        _repoMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _sut = new BandUpdatedConsumer(_repoMock.Object);
    }

    private static ConsumeContext<BandUpdatedIntegrationEvent> MockContext(Guid bandId, string name = "Death")
    {
        Mock<ConsumeContext<BandUpdatedIntegrationEvent>> ctx = new();
        ctx.Setup(x => x.Message).Returns(new BandUpdatedIntegrationEvent { BandId = bandId, Name = name, Status = 1, Countries = [] });
        ctx.Setup(x => x.CancellationToken).Returns(CancellationToken.None);
        return ctx.Object;
    }

    [Fact]
    public async Task Consume_ExistingBand_UpdatesFields()
    {
        Guid bandId = Guid.NewGuid();
        Band band = new() { BandId = bandId, BandName = "OldName" };
        _repoMock.Setup(x => x.GetByAsync<Band>(It.IsAny<Expression<Func<Band, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(band);

        await _sut.Consume(MockContext(bandId, "Death"));

        band.BandName.Should().Be("Death");
        _repoMock.Verify(x => x.Update(band), Times.Once);
        _repoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Consume_BandNotFound_CreatesNewBand()
    {
        _repoMock.Setup(x => x.GetByAsync<Band>(It.IsAny<Expression<Func<Band, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Band?)null);

        Guid bandId = Guid.NewGuid();
        await _sut.Consume(MockContext(bandId));

        _repoMock.Verify(x => x.AddAsync(It.Is<Band>(b => b.BandId == bandId), It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
