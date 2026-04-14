using FluentAssertions;
using Library.Application.Data;
using Library.Application.Exceptions;
using Library.Application.Services.Bands.Commands.DeleteBand;
using Library.ApplicationTests.Utils;
using Library.Domain.Enums;
using Library.Domain.Models;
using Library.Domain.ValueObjects;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.ApplicationTests.Bands;

public class DeleteBandHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly Mock<DbSet<Band>> _bandsDbSetMock;
    private readonly Mock<IStorageService> _storageMock;
    private readonly DeleteBandCommandHandler _handler;

    public DeleteBandHandlerTests()
    {
        _bandsDbSetMock = MockDbSetFactory.Create<Band>();
        _storageMock = new Mock<IStorageService>();

        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.Bands).Returns(_bandsDbSetMock.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _handler = new DeleteBandCommandHandler(_contextMock.Object, _storageMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingBand_RemovesBandAndReturnsSuccess()
    {
        Guid bandId = Guid.NewGuid();
        Band band = Band.Create("Death", null, null, BandActivity.Of(1983, null), BandStatus.Active, BandId.Of(bandId));
        _bandsDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Band?>(band));

        DeleteBandResult result = await _handler.Handle(new DeleteBandCommand(bandId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _bandsDbSetMock.Verify(x => x.Remove(band), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingBand_ThrowsBandNotFoundException()
    {
        _bandsDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Band?>(null));

        Func<Task> act = async () => await _handler.Handle(new DeleteBandCommand(Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<BandNotFoundException>();
    }

    [Fact]
    public async Task Handle_BandWithLogo_DeletesLogoFromStorage()
    {
        Guid bandId = Guid.NewGuid();
        Band band = Band.Create("Death", null, "bands/death/logo/abc.jpg", BandActivity.Of(1983, null), BandStatus.Active, BandId.Of(bandId));
        _bandsDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Band?>(band));

        await _handler.Handle(new DeleteBandCommand(bandId), CancellationToken.None);

        _storageMock.Verify(x => x.DeleteFileAsync("bands/death/logo/abc.jpg", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_BandWithoutLogo_DoesNotCallStorage()
    {
        Guid bandId = Guid.NewGuid();
        Band band = Band.Create("Death", null, null, BandActivity.Of(1983, null), BandStatus.Active, BandId.Of(bandId));
        _bandsDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Band?>(band));

        await _handler.Handle(new DeleteBandCommand(bandId), CancellationToken.None);

        _storageMock.Verify(x => x.DeleteFileAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }
}
