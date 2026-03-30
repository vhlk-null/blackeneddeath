using FluentAssertions;
using Library.Application.Data;
using Library.Application.Exceptions;
using Library.Application.Services.GenreCards.Commands.RemoveGenreFromGenreCard;
using Library.ApplicationTests.Utils;
using Library.Domain.Exceptions;
using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.ApplicationTests.GenreCards;

public class RemoveGenreFromGenreCardHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly RemoveGenreFromGenreCardCommandHandler _handler;

    public RemoveGenreFromGenreCardHandlerTests()
    {
        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _handler = new RemoveGenreFromGenreCardCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingGenre_RemovesAndReturnsSuccess()
    {
        var cardId = Guid.NewGuid();
        var genreId = Guid.NewGuid();
        var card = GenreCard.Create(GenreCardId.Of(cardId), "Doom", "Slow and heavy");
        card.AddGenre(GenreId.Of(genreId));

        var cardsDbSet = MockDbSetFactory.Create(card);
        _contextMock.Setup(x => x.GenreCards).Returns(cardsDbSet.Object);

        var result = await _handler.Handle(
            new RemoveGenreFromGenreCardCommand(cardId, genreId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        card.GenreCardGenres.Should().BeEmpty();
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CardNotFound_ThrowsGenreCardNotFoundException()
    {
        var cardsDbSet = MockDbSetFactory.Create<GenreCard>();
        _contextMock.Setup(x => x.GenreCards).Returns(cardsDbSet.Object);

        var act = async () => await _handler.Handle(
            new RemoveGenreFromGenreCardCommand(Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<GenreCardNotFoundException>();
    }

    [Fact]
    public async Task Handle_GenreNotOnCard_ThrowsDomainException()
    {
        var cardId = Guid.NewGuid();
        var card = GenreCard.Create(GenreCardId.Of(cardId), "Technical", "Technical metal");

        var cardsDbSet = MockDbSetFactory.Create(card);
        _contextMock.Setup(x => x.GenreCards).Returns(cardsDbSet.Object);

        var act = async () => await _handler.Handle(
            new RemoveGenreFromGenreCardCommand(cardId, Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<DomainException>();
    }
}
