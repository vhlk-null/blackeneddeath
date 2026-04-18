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

public class BandCreatedConsumerTests
{
    private readonly Mock<IRepository<UserContentContext>> _repoMock = new();
    private readonly BandCreatedConsumer _sut;

    public BandCreatedConsumerTests()
    {
        _repoMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _sut = new BandCreatedConsumer(_repoMock.Object);
    }

    private static ConsumeContext<BandCreatedIntegrationEvent> MockContext(Guid bandId, string name = "Death")
    {
        Mock<ConsumeContext<BandCreatedIntegrationEvent>> ctx = new();
        ctx.Setup(x => x.Message).Returns(new BandCreatedIntegrationEvent { BandId = bandId, Name = name, Status = 1, Countries = [] });
        ctx.Setup(x => x.CancellationToken).Returns(CancellationToken.None);
        return ctx.Object;
    }

    [Fact]
    public async Task Consume_NewBand_AddsAndSaves()
    {
        _repoMock.Setup(x => x.GetByAsync<Band>(It.IsAny<Expression<Func<Band, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Band?)null);

        Guid bandId = Guid.NewGuid();
        await _sut.Consume(MockContext(bandId));

        _repoMock.Verify(x => x.AddAsync(
            It.Is<Band>(b => b.BandId == bandId && b.BandName == "Death"),
            It.IsAny<CancellationToken>()), Times.Once);
        _repoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Consume_BandAlreadyExists_DoesNothing()
    {
        Guid bandId = Guid.NewGuid();
        _repoMock.Setup(x => x.GetByAsync<Band>(It.IsAny<Expression<Func<Band, bool>>>(), It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Band { BandId = bandId, BandName = "Death" });

        await _sut.Consume(MockContext(bandId));

        _repoMock.Verify(x => x.AddAsync(It.IsAny<Band>(), It.IsAny<CancellationToken>()), Times.Never);
        _repoMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}
