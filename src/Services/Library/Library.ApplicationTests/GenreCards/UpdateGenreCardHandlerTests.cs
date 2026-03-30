using FluentAssertions;
using Library.Application.Data;
using Library.Application.Exceptions;
using Library.Application.Services.GenreCards.Commands.UpdateGenreCard;
using Library.ApplicationTests.Utils;
using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Moq;
using Xunit;

namespace Library.ApplicationTests.GenreCards;

public class UpdateGenreCardHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly Mock<IStorageService> _storageMock;
    private readonly UpdateGenreCardCommandHandler _handler;

    public UpdateGenreCardHandlerTests()
    {
        _contextMock = new Mock<ILibraryDbContext>();
        _storageMock = new Mock<IStorageService>();
        _contextMock.Setup(x => x.GenreCards).Returns(MockDbSetFactory.Create<GenreCard>().Object);
        _contextMock.Setup(x => x.Genres).Returns(MockDbSetFactory.Create<Genre>().Object);
        _contextMock.Setup(x => x.Tags).Returns(MockDbSetFactory.Create<Tag>().Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _handler = new UpdateGenreCardCommandHandler(_contextMock.Object, _storageMock.Object);
    }

    [Fact]
    public async Task Handle_WithoutNewCoverImage_KeepsExistingCoverUrl()
    {
        var cardId = Guid.NewGuid();
        var card = GenreCard.Create(GenreCardId.Of(cardId), "Old Name", "Old description", "genres/old/existing-key");
        _contextMock.Setup(x => x.GenreCards).Returns(MockDbSetFactory.Create(card).Object);

        var result = await _handler.Handle(
            new UpdateGenreCardCommand(cardId, "New Name", "New description", 1, [], []),
            CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        card.Name.Should().Be("New Name");
        card.CoverUrl.Should().Be("genres/old/existing-key");
        _storageMock.Verify(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _storageMock.Verify(x => x.DeleteFileAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_WithNewCoverImage_DeletesOldAndUploadsNew()
    {
        var cardId = Guid.NewGuid();
        const string oldKey = "genres/doom/old-key";
        const string newKey = "genres/doom/new-key";
        var card = GenreCard.Create(GenreCardId.Of(cardId), "Doom", "Heavy", oldKey);
        _contextMock.Setup(x => x.GenreCards).Returns(MockDbSetFactory.Create(card).Object);
        _storageMock
            .Setup(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(newKey);

        await _handler.Handle(
            new UpdateGenreCardCommand(cardId, "Doom", "Heavy", 1, [], [], Stream.Null, "image/jpeg", "new.jpg"),
            CancellationToken.None);

        _storageMock.Verify(x => x.DeleteFileAsync(oldKey, It.IsAny<CancellationToken>()), Times.Once);
        _storageMock.Verify(x => x.UploadFileAsync("genres/doom", It.IsAny<string>(), Stream.Null, "image/jpeg", It.IsAny<CancellationToken>()), Times.Once);
        card.CoverUrl.Should().Be(newKey);
    }

    [Fact]
    public async Task Handle_WithNewCoverImageAndNoPreviousCover_UploadsWithoutDeleting()
    {
        var cardId = Guid.NewGuid();
        var card = GenreCard.Create(GenreCardId.Of(cardId), "Thrash", "Fast", null);
        _contextMock.Setup(x => x.GenreCards).Returns(MockDbSetFactory.Create(card).Object);
        _storageMock
            .Setup(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync("genres/thrash/new-key");

        await _handler.Handle(
            new UpdateGenreCardCommand(cardId, "Thrash", "Fast", 1, [], [], Stream.Null, "image/jpeg", "cover.jpg"),
            CancellationToken.None);

        _storageMock.Verify(x => x.DeleteFileAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
        _storageMock.Verify(x => x.UploadFileAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<Stream>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingCard_ThrowsGenreCardNotFoundException()
    {
        var act = async () => await _handler.Handle(
            new UpdateGenreCardCommand(Guid.NewGuid(), "Name", "Desc", 1, [], []),
            CancellationToken.None);

        await act.Should().ThrowAsync<GenreCardNotFoundException>();
    }
}
