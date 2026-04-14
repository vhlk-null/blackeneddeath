using FluentAssertions;
using Library.Application.Data;
using Library.Application.Exceptions;
using Library.Application.Services.Genres.Commands.UpdateGenre;
using Library.ApplicationTests.Utils;
using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.ApplicationTests.Genres;

public class UpdateGenreHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly Mock<DbSet<Genre>> _genresDbSetMock;
    private readonly UpdateGenreCommandHandler _handler;

    public UpdateGenreHandlerTests()
    {
        _genresDbSetMock = MockDbSetFactory.Create<Genre>();
        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.Genres).Returns(_genresDbSetMock.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _handler = new UpdateGenreCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingGenre_UpdatesNameAndReturnsSuccess()
    {
        Guid genreId = Guid.NewGuid();
        Genre genre = Genre.Create(GenreId.Of(genreId), "Death Metal");
        _genresDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Genre?>(genre));

        UpdateGenreResult result = await _handler.Handle(
            new UpdateGenreCommand(genreId, "Technical Death Metal", null), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        genre.Name.Should().Be("Technical Death Metal");
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingGenre_ThrowsGenreNotFoundException()
    {
        _genresDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Genre?>(null));

        Func<Task> act = async () => await _handler.Handle(
            new UpdateGenreCommand(Guid.NewGuid(), "Technical Death Metal", null), CancellationToken.None);

        await act.Should().ThrowAsync<GenreNotFoundException>();
    }

    [Fact]
    public async Task Handle_WithParentGenreId_SetsParentGenreId()
    {
        Guid genreId = Guid.NewGuid();
        Guid parentId = Guid.NewGuid();
        Genre genre = Genre.Create(GenreId.Of(genreId), "Technical Death Metal");
        _genresDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Genre?>(genre));

        await _handler.Handle(new UpdateGenreCommand(genreId, "Technical Death Metal", parentId), CancellationToken.None);

        genre.ParentGenreId.Should().Be(GenreId.Of(parentId));
    }
}
