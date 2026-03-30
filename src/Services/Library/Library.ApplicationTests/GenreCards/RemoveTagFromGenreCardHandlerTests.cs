using FluentAssertions;
using Library.Application.Data;
using Library.Application.Exceptions;
using Library.Application.Services.GenreCards.Commands.RemoveTagFromGenreCard;
using Library.ApplicationTests.Utils;
using Library.Domain.Exceptions;
using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.ApplicationTests.GenreCards;

public class RemoveTagFromGenreCardHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly RemoveTagFromGenreCardCommandHandler _handler;

    public RemoveTagFromGenreCardHandlerTests()
    {
        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _handler = new RemoveTagFromGenreCardCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingTag_RemovesAndReturnsSuccess()
    {
        var cardId = Guid.NewGuid();
        var tagId = Guid.NewGuid();
        var card = GenreCard.Create(GenreCardId.Of(cardId), "Grind", "Fast and brutal");
        card.AddTag(TagId.Of(tagId));

        var cardsDbSet = MockDbSetFactory.Create(card);
        _contextMock.Setup(x => x.GenreCards).Returns(cardsDbSet.Object);

        var result = await _handler.Handle(
            new RemoveTagFromGenreCardCommand(cardId, tagId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        card.GenreCardTags.Should().BeEmpty();
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CardNotFound_ThrowsGenreCardNotFoundException()
    {
        var cardsDbSet = MockDbSetFactory.Create<GenreCard>();
        _contextMock.Setup(x => x.GenreCards).Returns(cardsDbSet.Object);

        var act = async () => await _handler.Handle(
            new RemoveTagFromGenreCardCommand(Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<GenreCardNotFoundException>();
    }

    [Fact]
    public async Task Handle_TagNotOnCard_ThrowsDomainException()
    {
        var cardId = Guid.NewGuid();
        var card = GenreCard.Create(GenreCardId.Of(cardId), "Avant-Garde", "Experimental and weird");

        var cardsDbSet = MockDbSetFactory.Create(card);
        _contextMock.Setup(x => x.GenreCards).Returns(cardsDbSet.Object);

        var act = async () => await _handler.Handle(
            new RemoveTagFromGenreCardCommand(cardId, Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<DomainException>();
    }
}
