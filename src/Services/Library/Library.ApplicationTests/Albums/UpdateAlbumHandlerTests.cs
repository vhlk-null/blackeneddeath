using FluentAssertions;
using Library.Application.Data;
using Library.Application.Dtos;
using Library.Application.Exceptions;
using Library.Application.Services.Albums.Commands.UpdateAlbum;
using Library.ApplicationTests.Utils;
using Library.Domain.Enums;
using Library.Domain.Models;
using Library.Domain.ValueObjects;
using Library.Domain.ValueObjects.Ids;
using Moq;
using Xunit;

namespace Library.ApplicationTests.Albums;

public class UpdateAlbumHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly Mock<IStorageService> _storageMock;
    private readonly UpdateAlbumCommandHandler _handler;

    public UpdateAlbumHandlerTests()
    {
        _storageMock = new Mock<IStorageService>();
        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _contextMock.Setup(x => x.Bands).Returns(MockDbSetFactory.Create<Band>().Object);
        _contextMock.Setup(x => x.Labels).Returns(MockDbSetFactory.Create<Label>().Object);
        _contextMock.Setup(x => x.Tracks).Returns(MockDbSetFactory.Create<Track>().Object);
        _handler = new UpdateAlbumCommandHandler(_contextMock.Object, _storageMock.Object);
    }

    private static UpdateAlbumDto SimpleDto(Guid albumId, string title = "Symbolic") =>
        new(albumId, title, 1995, AlbumType.FullLength, AlbumFormat.CD,
            null, null, [], null, [Guid.NewGuid()], [], [], [], null);

    [Fact]
    public async Task Handle_NonExistingAlbum_ThrowsAlbumNotFoundException()
    {
        _contextMock.Setup(x => x.Albums).Returns(MockDbSetFactory.Create<Album>().Object);

        Func<Task> act = async () => await _handler.Handle(
            new UpdateAlbumCommand(SimpleDto(Guid.NewGuid())), CancellationToken.None);

        await act.Should().ThrowAsync<AlbumNotFoundException>();
    }

    [Fact]
    public async Task Handle_ExistingAlbum_UpdatesAndReturnsSuccess()
    {
        Guid albumId = Guid.NewGuid();
        Album album = Album.Create("Symbolic", AlbumType.FullLength, AlbumRelease.Of(1995, AlbumFormat.CD), null, null, AlbumId.Of(albumId));
        _contextMock.Setup(x => x.Albums).Returns(MockDbSetFactory.Create(album).Object);

        UpdateAlbumResult result = await _handler.Handle(
            new UpdateAlbumCommand(SimpleDto(albumId, "Symbolic")), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithNewCover_ReplacesOldCoverInStorage()
    {
        Guid albumId = Guid.NewGuid();
        Album album = Album.Create("Symbolic", AlbumType.FullLength, AlbumRelease.Of(1995, AlbumFormat.CD), "albums/symbolic/old.jpg", null, AlbumId.Of(albumId));
        _contextMock.Setup(x => x.Albums).Returns(MockDbSetFactory.Create(album).Object);

        const string newKey = "albums/symbolic/new.jpg";
        _storageMock
            .Setup(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newKey);

        await _handler.Handle(
            new UpdateAlbumCommand(SimpleDto(albumId), Stream.Null, "image/jpeg", "new.jpg"), CancellationToken.None);

        _storageMock.Verify(x => x.DeleteFileAsync("albums/symbolic/old.jpg", It.IsAny<CancellationToken>()), Times.Once);
        _storageMock.Verify(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>(), Stream.Null, "image/jpeg", It.IsAny<CancellationToken>()), Times.Once);
    }
}
