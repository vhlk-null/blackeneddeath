using FluentAssertions;
using Library.Application.Data;
using Library.Application.Dtos;
using Library.Application.Exceptions;
using Library.Application.Services.Bands.Commands.UpdateBand;
using Library.ApplicationTests.Utils;
using Library.Domain.Enums;
using Library.Domain.Models;
using Library.Domain.ValueObjects;
using Library.Domain.ValueObjects.Ids;
using Moq;
using Xunit;

namespace Library.ApplicationTests.Bands;

public class UpdateBandHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly Mock<IStorageService> _storageMock;
    private readonly UpdateBandCommandHandler _handler;

    public UpdateBandHandlerTests()
    {
        _storageMock = new Mock<IStorageService>();
        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _handler = new UpdateBandCommandHandler(_contextMock.Object, _storageMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingBand_UpdatesAndReturnsSuccess()
    {
        Guid bandId = Guid.NewGuid();
        Band band = Band.Create("Death", null, null, BandActivity.Of(1983, null), BandStatus.Active, BandId.Of(bandId));
        _contextMock.Setup(x => x.Bands).Returns(MockDbSetFactory.Create(band).Object);

        UpdateBandCommand command = new(new UpdateBandDto(
            bandId, "Death", "Legendary band", 1983, null, BandStatus.Active, [], []));

        UpdateBandResult result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingBand_ThrowsBandNotFoundException()
    {
        _contextMock.Setup(x => x.Bands).Returns(MockDbSetFactory.Create<Band>().Object);

        UpdateBandCommand command = new(new UpdateBandDto(
            Guid.NewGuid(), "Death", null, 1983, null, BandStatus.Active, [], []));

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<BandNotFoundException>();
    }

    [Fact]
    public async Task Handle_WithNewLogo_ReplacesOldLogoInStorage()
    {
        Guid bandId = Guid.NewGuid();
        Band band = Band.Create("Death", null, "bands/death/logo/old.jpg", BandActivity.Of(1983, null), BandStatus.Active, BandId.Of(bandId));
        _contextMock.Setup(x => x.Bands).Returns(MockDbSetFactory.Create(band).Object);

        const string newKey = "bands/death/logo/new.jpg";
        _storageMock
            .Setup(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newKey);

        UpdateBandCommand command = new(
            new UpdateBandDto(bandId, "Death", null, 1983, null, BandStatus.Active, [], []),
            Stream.Null, "image/jpeg", "new.jpg");

        await _handler.Handle(command, CancellationToken.None);

        _storageMock.Verify(x => x.DeleteFileAsync("bands/death/logo/old.jpg", It.IsAny<CancellationToken>()), Times.Once);
        _storageMock.Verify(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>(), Stream.Null, "image/jpeg", It.IsAny<CancellationToken>()), Times.Once);
    }
}
