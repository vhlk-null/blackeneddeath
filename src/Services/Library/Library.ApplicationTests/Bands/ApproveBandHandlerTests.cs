using FluentAssertions;
using Library.Application.Data;
using Library.Application.Exceptions;
using Library.Application.Services.Bands.Commands.ApproveBand;
using Library.ApplicationTests.Utils;
using Library.Domain.Enums;
using Library.Domain.Models;
using Library.Domain.ValueObjects;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.ApplicationTests.Bands;

public class ApproveBandHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly Mock<DbSet<Band>> _bandsDbSetMock;
    private readonly ApproveBandCommandHandler _handler;

    public ApproveBandHandlerTests()
    {
        _bandsDbSetMock = MockDbSetFactory.Create<Band>();
        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.Bands).Returns(_bandsDbSetMock.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _handler = new ApproveBandCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingBand_ApprovesAndReturnsSuccess()
    {
        Guid bandId = Guid.NewGuid();
        Band band = Band.Create("Death", null, null, BandActivity.Of(1983, null), BandStatus.Active, BandId.Of(bandId));
        _bandsDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Band?>(band));

        ApproveBandResult result = await _handler.Handle(new ApproveBandCommand(bandId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        band.IsApproved.Should().BeTrue();
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingBand_ThrowsBandNotFoundException()
    {
        _bandsDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Band?>(null));

        Func<Task> act = async () => await _handler.Handle(new ApproveBandCommand(Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<BandNotFoundException>();
    }
}
