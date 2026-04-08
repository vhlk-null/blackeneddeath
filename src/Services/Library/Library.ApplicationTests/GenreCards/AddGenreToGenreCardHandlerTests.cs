using FluentAssertions;
using Library.Application.Data;
using Library.Application.Exceptions;
using Library.Application.Services.GenreCards.Commands.AddGenreToGenreCard;
using Library.ApplicationTests.Utils;
using Library.Domain.Exceptions;
using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.ApplicationTests.GenreCards;

public class AddGenreToGenreCardHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly AddGenreToGenreCardCommandHandler _handler;

    public AddGenreToGenreCardHandlerTests()
    {
        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _handler = new AddGenreToGenreCardCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_AddsGenreAndReturnsSuccess()
    {
        Guid cardId = Guid.NewGuid();
        Guid genreId = Guid.NewGuid();
        GenreCard card = GenreCard.Create(GenreCardId.Of(cardId), "Melodic", "Melodic metal");
        Genre genre = Genre.Create(GenreId.Of(genreId), "Melodic Black Metal");

        Mock<DbSet<GenreCard>> cardsDbSet = MockDbSetFactory.Create(card);
        Mock<DbSet<Genre>> genresDbSet = MockDbSetFactory.Create(genre);
        _contextMock.Setup(x => x.GenreCards).Returns(cardsDbSet.Object);
        _contextMock.Setup(x => x.Genres).Returns(genresDbSet.Object);

        AddGenreToGenreCardResult result = await _handler.Handle(
            new AddGenreToGenreCardCommand(cardId, genreId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        card.GenreCardGenres.Should().ContainSingle(g => g.GenreId == GenreId.Of(genreId));
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CardNotFound_ThrowsGenreCardNotFoundException()
    {
        Guid genreId = Guid.NewGuid();
        Mock<DbSet<GenreCard>> cardsDbSet = MockDbSetFactory.Create<GenreCard>();
        Mock<DbSet<Genre>> genresDbSet = MockDbSetFactory.Create(Genre.Create(GenreId.Of(genreId), "Some Genre"));
        _contextMock.Setup(x => x.GenreCards).Returns(cardsDbSet.Object);
        _contextMock.Setup(x => x.Genres).Returns(genresDbSet.Object);

        Func<Task<AddGenreToGenreCardResult>> act = async () => await _handler.Handle(
            new AddGenreToGenreCardCommand(Guid.NewGuid(), genreId), CancellationToken.None);

        await act.Should().ThrowAsync<GenreCardNotFoundException>();
    }

    [Fact]
    public async Task Handle_GenreNotFound_ThrowsGenreNotFoundException()
    {
        Guid cardId = Guid.NewGuid();
        GenreCard card = GenreCard.Create(GenreCardId.Of(cardId), "Brutal", "Brutal death");
        Mock<DbSet<GenreCard>> cardsDbSet = MockDbSetFactory.Create(card);
        Mock<DbSet<Genre>> genresDbSet = MockDbSetFactory.Create<Genre>();
        _contextMock.Setup(x => x.GenreCards).Returns(cardsDbSet.Object);
        _contextMock.Setup(x => x.Genres).Returns(genresDbSet.Object);

        Func<Task<AddGenreToGenreCardResult>> act = async () => await _handler.Handle(
            new AddGenreToGenreCardCommand(cardId, Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<GenreNotFoundException>();
    }

    [Fact]
    public async Task Handle_DuplicateGenre_ThrowsDomainException()
    {
        Guid cardId = Guid.NewGuid();
        Guid genreId = Guid.NewGuid();
        GenreCard card = GenreCard.Create(GenreCardId.Of(cardId), "Symphonic", "Symphonic metal");
        card.AddGenre(GenreId.Of(genreId));

        Genre genre = Genre.Create(GenreId.Of(genreId), "Symphonic Black Metal");
        Mock<DbSet<GenreCard>> cardsDbSet = MockDbSetFactory.Create(card);
        Mock<DbSet<Genre>> genresDbSet = MockDbSetFactory.Create(genre);
        _contextMock.Setup(x => x.GenreCards).Returns(cardsDbSet.Object);
        _contextMock.Setup(x => x.Genres).Returns(genresDbSet.Object);

        Func<Task<AddGenreToGenreCardResult>> act = async () => await _handler.Handle(
            new AddGenreToGenreCardCommand(cardId, genreId), CancellationToken.None);

        await act.Should().ThrowAsync<DomainException>();
    }
}
