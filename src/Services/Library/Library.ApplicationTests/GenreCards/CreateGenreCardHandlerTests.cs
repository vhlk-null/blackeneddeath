using FluentAssertions;
using Library.Application.Data;
using Library.Application.Services.GenreCards.Commands.CreateGenreCard;
using Library.ApplicationTests.Utils;
using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.ApplicationTests.GenreCards;

public class CreateGenreCardHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly Mock<IStorageService> _storageMock;
    private readonly Mock<DbSet<GenreCard>> _genreCardsDbSetMock;
    private readonly CreateGenreCardCommandHandler _handler;

    public CreateGenreCardHandlerTests()
    {
        _genreCardsDbSetMock = MockDbSetFactory.Create<GenreCard>();
        _contextMock = new Mock<ILibraryDbContext>();
        _storageMock = new Mock<IStorageService>();
        _contextMock.Setup(x => x.GenreCards).Returns(_genreCardsDbSetMock.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _handler = new CreateGenreCardCommandHandler(_contextMock.Object, _storageMock.Object);
    }

    [Fact]
    public async Task Handle_WithoutCoverImage_AddsCardWithNullCoverUrl()
    {
        var command = new CreateGenreCardCommand("Classic", "Classic heavy metal vibes", 1);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Id.Should().NotBeEmpty();
        _genreCardsDbSetMock.Verify(x => x.Add(It.Is<GenreCard>(c =>
            c.Name == "Classic" && c.CoverUrl == null)), Times.Once);
        _storageMock.Verify(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithCoverImage_UploadsToGenresFolderAndStoresKey()
    {
        const string publicId = "genres/war-metal/abc123";
        _storageMock
            .Setup(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(publicId);

        var command = new CreateGenreCardCommand("War Metal", "Bestial and primitive", 2,
            Stream.Null, "image/jpeg", "cover.jpg");

        await _handler.Handle(command, CancellationToken.None);

        _storageMock.Verify(x => x.UploadFileAsync(
            "genres/war-metal", It.IsAny<string>(), Stream.Null, "image/jpeg", It.IsAny<CancellationToken>()), Times.Once);
        _genreCardsDbSetMock.Verify(x => x.Add(It.Is<GenreCard>(c => c.CoverUrl == publicId)), Times.Once);
    }

    [Fact]
    public async Task Handle_WithCoverImage_UsesSlugifiedNameAsFolder()
    {
        _storageMock
            .Setup(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("genres/folk-oriental/xyz");

        var command = new CreateGenreCardCommand("Folk/Oriental", "Folk vibes", 3,
            Stream.Null, "image/png", "cover.png");

        await _handler.Handle(command, CancellationToken.None);

        _storageMock.Verify(x => x.UploadFileAsync(
            "genres/folk-oriental", It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
