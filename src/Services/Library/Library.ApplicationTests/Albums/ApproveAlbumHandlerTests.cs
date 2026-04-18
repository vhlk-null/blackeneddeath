using FluentAssertions;
using Library.Application.Data;
using Library.Application.Exceptions;
using Library.Application.Services.Albums.Commands.ApproveAlbum;
using Library.ApplicationTests.Utils;
using Library.Domain.Enums;
using Library.Domain.Models;
using Library.Domain.ValueObjects;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.ApplicationTests.Albums;

public class ApproveAlbumHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly Mock<DbSet<Album>> _albumsDbSetMock;
    private readonly ApproveAlbumCommandHandler _handler;

    public ApproveAlbumHandlerTests()
    {
        _albumsDbSetMock = MockDbSetFactory.Create<Album>();
        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.Albums).Returns(_albumsDbSetMock.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _handler = new ApproveAlbumCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingAlbum_ApprovesAndReturnsSuccess()
    {
        Guid albumId = Guid.NewGuid();
        Album album = Album.Create("Symbolic", AlbumType.FullLength, AlbumRelease.Of(1995, AlbumFormat.CD), null, null, AlbumId.Of(albumId));
        _albumsDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Album?>(album));

        ApproveAlbumResult result = await _handler.Handle(new ApproveAlbumCommand(albumId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        album.IsApproved.Should().BeTrue();
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingAlbum_ThrowsAlbumNotFoundException()
    {
        _albumsDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Album?>(null));

        Func<Task> act = async () => await _handler.Handle(new ApproveAlbumCommand(Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<AlbumNotFoundException>();
    }
}
