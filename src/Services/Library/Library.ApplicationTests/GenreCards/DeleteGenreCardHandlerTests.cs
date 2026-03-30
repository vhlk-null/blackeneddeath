using FluentAssertions;
using Library.Application.Data;
using Library.Application.Exceptions;
using Library.Application.Services.GenreCards.Commands.DeleteGenreCard;
using Library.ApplicationTests.Utils;
using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.ApplicationTests.GenreCards;

public class DeleteGenreCardHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly Mock<DbSet<GenreCard>> _genreCardsDbSetMock;
    private readonly DeleteGenreCardCommandHandler _handler;

    public DeleteGenreCardHandlerTests()
    {
        _genreCardsDbSetMock = MockDbSetFactory.Create<GenreCard>();
        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.GenreCards).Returns(_genreCardsDbSetMock.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _handler = new DeleteGenreCardCommandHandler(_contextMock.Object, Mock.Of<IStorageService>());
    }

    [Fact]
    public async Task Handle_ExistingCard_RemovesAndReturnsSuccess()
    {
        var cardId = Guid.NewGuid();
        var card = GenreCard.Create(GenreCardId.Of(cardId), "Thrash", "Fast and aggressive");
        _genreCardsDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<GenreCard?>(card));

        var result = await _handler.Handle(new DeleteGenreCardCommand(cardId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _genreCardsDbSetMock.Verify(x => x.Remove(card), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingCard_ThrowsGenreCardNotFoundException()
    {
        _genreCardsDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<GenreCard?>(null));

        var act = async () => await _handler.Handle(
            new DeleteGenreCardCommand(Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<GenreCardNotFoundException>();
    }
}
