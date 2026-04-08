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
        Guid cardId = Guid.NewGuid();
        Guid genreId = Guid.NewGuid();
        GenreCard card = GenreCard.Create(GenreCardId.Of(cardId), "Doom", "Slow and heavy");
        card.AddGenre(GenreId.Of(genreId));

        Mock<DbSet<GenreCard>> cardsDbSet = MockDbSetFactory.Create(card);
        _contextMock.Setup(x => x.GenreCards).Returns(cardsDbSet.Object);

        RemoveGenreFromGenreCardResult result = await _handler.Handle(
            new RemoveGenreFromGenreCardCommand(cardId, genreId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        card.GenreCardGenres.Should().BeEmpty();
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CardNotFound_ThrowsGenreCardNotFoundException()
    {
        Mock<DbSet<GenreCard>> cardsDbSet = MockDbSetFactory.Create<GenreCard>();
        _contextMock.Setup(x => x.GenreCards).Returns(cardsDbSet.Object);

        Func<Task<RemoveGenreFromGenreCardResult>> act = async () => await _handler.Handle(
            new RemoveGenreFromGenreCardCommand(Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<GenreCardNotFoundException>();
    }

    [Fact]
    public async Task Handle_GenreNotOnCard_ThrowsDomainException()
    {
        Guid cardId = Guid.NewGuid();
        GenreCard card = GenreCard.Create(GenreCardId.Of(cardId), "Technical", "Technical metal");

        Mock<DbSet<GenreCard>> cardsDbSet = MockDbSetFactory.Create(card);
        _contextMock.Setup(x => x.GenreCards).Returns(cardsDbSet.Object);

        Func<Task<RemoveGenreFromGenreCardResult>> act = async () => await _handler.Handle(
            new RemoveGenreFromGenreCardCommand(cardId, Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<DomainException>();
    }
}
