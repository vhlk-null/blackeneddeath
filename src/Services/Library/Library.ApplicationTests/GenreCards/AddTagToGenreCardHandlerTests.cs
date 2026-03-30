using FluentAssertions;
using Library.Application.Data;
using Library.Application.Exceptions;
using Library.Application.Services.GenreCards.Commands.AddTagToGenreCard;
using Library.ApplicationTests.Utils;
using Library.Domain.Exceptions;
using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.ApplicationTests.GenreCards;

public class AddTagToGenreCardHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly AddTagToGenreCardCommandHandler _handler;

    public AddTagToGenreCardHandlerTests()
    {
        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _handler = new AddTagToGenreCardCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_AddsTagAndReturnsSuccess()
    {
        var cardId = Guid.NewGuid();
        var tagId = Guid.NewGuid();
        var card = GenreCard.Create(GenreCardId.Of(cardId), "Cavernous", "Deep cave sounds");
        var tag = Tag.Create(TagId.Of(tagId), "Raw");

        var cardsDbSet = MockDbSetFactory.Create(card);
        var tagsDbSet = MockDbSetFactory.Create(tag);
        _contextMock.Setup(x => x.GenreCards).Returns(cardsDbSet.Object);
        _contextMock.Setup(x => x.Tags).Returns(tagsDbSet.Object);

        var result = await _handler.Handle(
            new AddTagToGenreCardCommand(cardId, tagId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        card.GenreCardTags.Should().ContainSingle(t => t.TagId == TagId.Of(tagId));
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CardNotFound_ThrowsGenreCardNotFoundException()
    {
        var tagId = Guid.NewGuid();
        var cardsDbSet = MockDbSetFactory.Create<GenreCard>();
        var tagsDbSet = MockDbSetFactory.Create(Tag.Create(TagId.Of(tagId), "Epic"));
        _contextMock.Setup(x => x.GenreCards).Returns(cardsDbSet.Object);
        _contextMock.Setup(x => x.Tags).Returns(tagsDbSet.Object);

        var act = async () => await _handler.Handle(
            new AddTagToGenreCardCommand(Guid.NewGuid(), tagId), CancellationToken.None);

        await act.Should().ThrowAsync<GenreCardNotFoundException>();
    }

    [Fact]
    public async Task Handle_TagNotFound_ThrowsTagNotFoundException()
    {
        var cardId = Guid.NewGuid();
        var card = GenreCard.Create(GenreCardId.Of(cardId), "Progressive", "Prog vibes");
        var cardsDbSet = MockDbSetFactory.Create(card);
        var tagsDbSet = MockDbSetFactory.Create<Tag>();
        _contextMock.Setup(x => x.GenreCards).Returns(cardsDbSet.Object);
        _contextMock.Setup(x => x.Tags).Returns(tagsDbSet.Object);

        var act = async () => await _handler.Handle(
            new AddTagToGenreCardCommand(cardId, Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<TagNotFoundException>();
    }

    [Fact]
    public async Task Handle_DuplicateTag_ThrowsDomainException()
    {
        var cardId = Guid.NewGuid();
        var tagId = Guid.NewGuid();
        var card = GenreCard.Create(GenreCardId.Of(cardId), "Blackened Death", "Extreme metal");
        card.AddTag(TagId.Of(tagId));

        var tag = Tag.Create(TagId.Of(tagId), "Atmospheric");
        var cardsDbSet = MockDbSetFactory.Create(card);
        var tagsDbSet = MockDbSetFactory.Create(tag);
        _contextMock.Setup(x => x.GenreCards).Returns(cardsDbSet.Object);
        _contextMock.Setup(x => x.Tags).Returns(tagsDbSet.Object);

        var act = async () => await _handler.Handle(
            new AddTagToGenreCardCommand(cardId, tagId), CancellationToken.None);

        await act.Should().ThrowAsync<DomainException>();
    }
}
