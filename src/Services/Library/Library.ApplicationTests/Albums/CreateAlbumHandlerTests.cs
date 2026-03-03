using FluentAssertions;
using Library.Application.Albums.Commands.CreateAlbum;
using Library.Application.Data;
using Library.Application.Dtos;
using Library.ApplicationTests.Utils;
using Library.Domain.Enums;
using Library.Domain.Models;
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
    private readonly Mock<DbSet<Band>> _bandsDbSetMock;
    private readonly CreateAlbumHandler _handler;

    public CreateAlbumHandlerTests()
    {
        _albumsDbSetMock = MockDbSetFactory.Create<Album>();
        _tracksDbSetMock = MockDbSetFactory.Create<Track>();
        _bandsDbSetMock  = MockDbSetFactory.Create<Band>();

        _tracksDbSetMock
            .Setup(x => x.AddRangeAsync(It.IsAny<IEnumerable<Track>>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.Albums).Returns(_albumsDbSetMock.Object);
        _contextMock.Setup(x => x.Tracks).Returns(_tracksDbSetMock.Object);
        _contextMock.Setup(x => x.Bands).Returns(_bandsDbSetMock.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _handler = new CreateAlbumHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_AddsAlbumAndReturnsId()
    {
        var command = new CreateAlbumCommand(new AlbumDto(
            Guid.NewGuid(), "Symbolic", 1995, null,
            AlbumType.FullLength, AlbumFormat.CD, "Roadrunner Records",
            [], [], [], [], []));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Id.Should().NotBeEmpty();
        _albumsDbSetMock.Verify(x => x.Add(It.IsAny<Album>()), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CommandWithNewBand_AddsAlbumWithBand()
    {
        var command = new CreateAlbumCommand(new AlbumDto(
            Guid.NewGuid(), "Symbolic", 1995, null,
            AlbumType.FullLength, AlbumFormat.CD, "Roadrunner Records",
            [new BandSummaryDto(null, "Death")],
            [], [], [], []));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Id.Should().NotBeEmpty();
        _albumsDbSetMock.Verify(x => x.Add(It.Is<Album>(a => a.AlbumBands.Any())), Times.Once);
        _bandsDbSetMock.Verify(x => x.Add(It.IsAny<Band>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CommandWithGenres_AddsAlbumWithGenres()
    {
        var genreId = Guid.NewGuid();
        var genre = Genre.Create(GenreId.Of(genreId), "Death Metal");
        var genresDbSetMock = MockDbSetFactory.Create<Genre>(genre);
        _contextMock.Setup(x => x.Genres).Returns(genresDbSetMock.Object);

        var command = new CreateAlbumCommand(new AlbumDto(
            Guid.NewGuid(), "Symbolic", 1995, null,
            AlbumType.FullLength, AlbumFormat.CD, "Roadrunner Records",
            [], [], [], [],
            [new GenreDto(genreId, "Death Metal", true)]));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Id.Should().NotBeEmpty();
        _albumsDbSetMock.Verify(x => x.Add(It.Is<Album>(a => a.AlbumGenres.Any())), Times.Once);
    }
}
