using FluentAssertions;
using Library.Application.Data;
using Library.Application.Dtos;
using Library.Application.Exceptions;
using Library.Application.Services.Albums.Commands.UpdateAlbum;
using Library.ApplicationTests.Utils;
using Library.Domain.Enums;
using Library.Domain.Models;
using Library.Domain.Models.JoinTables;
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
        SetupDefaultMocks();
        _handler = new UpdateAlbumCommandHandler(_contextMock.Object, _storageMock.Object);
    }

    private void SetupDefaultMocks()
    {
        _contextMock.Setup(x => x.Tracks).Returns(MockDbSetFactory.Create<Track>().Object);
        _contextMock.Setup(x => x.Bands).Returns(MockDbSetFactory.Create<Band>().Object);
        _contextMock.Setup(x => x.Labels).Returns(MockDbSetFactory.Create<Label>().Object);
        _contextMock.Setup(x => x.AlbumBands).Returns(MockDbSetFactory.Create<AlbumBand>().Object);
        _contextMock.Setup(x => x.AlbumGenres).Returns(MockDbSetFactory.Create<AlbumGenre>().Object);
        _contextMock.Setup(x => x.AlbumCountries).Returns(MockDbSetFactory.Create<AlbumCountry>().Object);
        _contextMock.Setup(x => x.AlbumTracks).Returns(MockDbSetFactory.Create<AlbumTrack>().Object);
        _contextMock.Setup(x => x.AlbumTags).Returns(MockDbSetFactory.Create<AlbumTag>().Object);
        _contextMock.Setup(x => x.StreamingLinks).Returns(MockDbSetFactory.Create<StreamingLink>().Object);
    }

    private static Album CreateAlbum(Guid id, string? coverUrl = null) =>
        Album.Create("Symbolic", AlbumType.FullLength,
            AlbumRelease.Of(1995, AlbumFormat.CD), coverUrl, null, AlbumId.Of(id));

    private static UpdateAlbumDto SimpleUpdateDto(Guid id, string title = "Human") =>
        new(id, title, 1991, AlbumType.FullLength, AlbumFormat.CD,
            null, null, [], null, [], [], [], [], null);

    [Fact]
    public async Task Handle_ExistingAlbum_UpdatesAndReturnsSuccess()
    {
        Guid albumId = Guid.NewGuid();
        _contextMock.Setup(x => x.Albums).Returns(MockDbSetFactory.Create(CreateAlbum(albumId)).Object);

        UpdateAlbumResult result = await _handler.Handle(
            new UpdateAlbumCommand(SimpleUpdateDto(albumId)), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingAlbum_ThrowsAlbumNotFoundException()
    {
        _contextMock.Setup(x => x.Albums).Returns(MockDbSetFactory.Create<Album>().Object);

        Func<Task> act = async () => await _handler.Handle(
            new UpdateAlbumCommand(SimpleUpdateDto(Guid.NewGuid())), CancellationToken.None);

        await act.Should().ThrowAsync<AlbumNotFoundException>();
    }

    [Fact]
    public async Task Handle_WithNewCover_ReplacesOldCoverInStorage()
    {
        Guid albumId = Guid.NewGuid();
        Album album = CreateAlbum(albumId, "albums/symbolic/old.jpg");
        _contextMock.Setup(x => x.Albums).Returns(MockDbSetFactory.Create(album).Object);

        const string newKey = "albums/human/new.jpg";
        _storageMock
            .Setup(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newKey);

        UpdateAlbumCommand command = new(
            SimpleUpdateDto(albumId),
            Stream.Null, "image/jpeg", "cover.jpg");

        await _handler.Handle(command, CancellationToken.None);

        _storageMock.Verify(x => x.DeleteFileAsync("albums/symbolic/old.jpg", It.IsAny<CancellationToken>()), Times.Once);
        _storageMock.Verify(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>(), Stream.Null, "image/jpeg", It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_WithLabelId_ValidatesLabelExists()
    {
        Guid albumId = Guid.NewGuid();
        Guid missingLabelId = Guid.NewGuid();
        _contextMock.Setup(x => x.Albums).Returns(MockDbSetFactory.Create(CreateAlbum(albumId)).Object);

        UpdateAlbumCommand command = new(new UpdateAlbumDto(
            albumId, "Human", 1991, AlbumType.FullLength, AlbumFormat.CD,
            [missingLabelId], null, [], null, [], [], [], [], null));

        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        await act.Should().ThrowAsync<LabelNotFoundException>();
    }
}
