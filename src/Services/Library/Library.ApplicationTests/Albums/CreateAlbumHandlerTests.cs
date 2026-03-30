using FluentAssertions;
using Library.Application.Data;
using Library.Application.Dtos;
using Library.Application.Services.Albums.Commands.CreateAlbum;
using Library.ApplicationTests.Utils;
using Library.Domain.Enums;
using Library.Domain.Models;
using Library.Domain.ValueObjects;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.ApplicationTests.Albums;

public class CreateAlbumHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly Mock<DbSet<Album>> _albumsDbSetMock;
    private readonly Mock<DbSet<Track>> _tracksDbSetMock;
    private readonly CreateAlbumHandler _handler;

    public CreateAlbumHandlerTests()
    {
        _albumsDbSetMock = MockDbSetFactory.Create<Album>();
        _tracksDbSetMock = MockDbSetFactory.Create<Track>();

        _tracksDbSetMock
            .Setup(x => x.AddRangeAsync(It.IsAny<IEnumerable<Track>>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.Albums).Returns(_albumsDbSetMock.Object);
        _contextMock.Setup(x => x.Tracks).Returns(_tracksDbSetMock.Object);
        _contextMock.Setup(x => x.Bands).Returns(MockDbSetFactory.Create<Band>().Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _handler = new CreateAlbumHandler(_contextMock.Object, Mock.Of<IStorageService>());
    }

    [Fact]
    public async Task Handle_ValidCommand_AddsAlbumAndReturnsId()
    {
        var command = new CreateAlbumCommand(new CreateAlbumDto(
            "Symbolic", 1995, AlbumType.FullLength, AlbumFormat.CD,
            null, [], [], [], [], [], null));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Id.Should().NotBeEmpty();
        _albumsDbSetMock.Verify(x => x.Add(It.IsAny<Album>()), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CommandWithExistingBand_AddsAlbumWithBandLinked()
    {
        var bandId = Guid.NewGuid();
        var band = Band.Create("Death", null, null, BandActivity.Of(1983, null), BandStatus.Active, BandId.Of(bandId));
        _contextMock.Setup(x => x.Bands).Returns(MockDbSetFactory.Create(band).Object);

        var command = new CreateAlbumCommand(new CreateAlbumDto(
            "Symbolic", 1995, AlbumType.FullLength, AlbumFormat.CD,
            null, [bandId], [], [], [], [], null));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Id.Should().NotBeEmpty();
        _albumsDbSetMock.Verify(x => x.Add(It.Is<Album>(a => a.AlbumBands.Any())), Times.Once);
    }

    [Fact]
    public async Task Handle_CommandWithGenres_AddsAlbumWithGenres()
    {
        var genreId = Guid.NewGuid();
        var genre = Genre.Create(GenreId.Of(genreId), "Death Metal");
        _contextMock.Setup(x => x.Genres).Returns(MockDbSetFactory.Create(genre).Object);

        var command = new CreateAlbumCommand(new CreateAlbumDto(
            "Symbolic", 1995, AlbumType.FullLength, AlbumFormat.CD,
            null, [], [], [genreId], [], [], null));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Id.Should().NotBeEmpty();
        _albumsDbSetMock.Verify(x => x.Add(It.Is<Album>(a => a.AlbumGenres.Any())), Times.Once);
    }
}
