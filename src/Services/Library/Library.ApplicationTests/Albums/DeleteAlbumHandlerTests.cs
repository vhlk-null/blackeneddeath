using FluentAssertions;
using Library.Application.Data;
using Library.Application.Exceptions;
using Library.Application.Services.Albums.Commands.DeleteAlbums;
using Library.ApplicationTests.Utils;
using Library.Domain.Enums;
using Library.Domain.Models;
using Library.Domain.ValueObjects;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.ApplicationTests.Albums;

public class DeleteAlbumHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly Mock<DbSet<Album>> _albumsDbSetMock;
    private readonly DeleteAlbumCommandHandler _handler;

    public DeleteAlbumHandlerTests()
    {
        _albumsDbSetMock = MockDbSetFactory.Create<Album>();
        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.Albums).Returns(_albumsDbSetMock.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _handler = new DeleteAlbumCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingAlbum_RemovesAlbumAndReturnsSuccess()
    {
        var albumId = Guid.NewGuid();
        var album = Album.Create(
            "Symbolic", AlbumType.FullLength,
            AlbumRelease.Of(1995, AlbumFormat.CD),
            null, null, AlbumId.Of(albumId));
        _albumsDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Album?>(album));

        var result = await _handler.Handle(new DeleteAlbumCommand(albumId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _albumsDbSetMock.Verify(x => x.Remove(album), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingAlbum_ThrowsAlbumNotFoundException()
    {
        _albumsDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Album?>(null));

        var act = async () => await _handler.Handle(
            new DeleteAlbumCommand(Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<AlbumNotFoundException>();
    }
}
