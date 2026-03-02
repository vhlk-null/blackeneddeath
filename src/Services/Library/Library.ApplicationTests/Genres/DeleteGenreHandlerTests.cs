using FluentAssertions;
using Library.Application.Data;
using Library.Application.Exceptions;
using Library.Application.Genres.Commands.DeleteGenre;
using Library.ApplicationTests.Utils;
using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.ApplicationTests.Genres;

public class DeleteGenreHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly Mock<DbSet<Genre>> _genresDbSetMock;
    private readonly DeleteGenreCommandHandler _handler;

    public DeleteGenreHandlerTests()
    {
        _genresDbSetMock = MockDbSetFactory.Create<Genre>();
        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.Genres).Returns(_genresDbSetMock.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _handler = new DeleteGenreCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingGenre_RemovesGenreAndReturnsSuccess()
    {
        var genreId = Guid.NewGuid();
        var genre = Genre.Create(GenreId.Of(genreId), "Death Metal", null);
        _genresDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Genre?>(genre));

        var result = await _handler.Handle(new DeleteGenreCommand(genreId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _genresDbSetMock.Verify(x => x.Remove(genre), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingGenre_ThrowsGenreNotFoundException()
    {
        _genresDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Genre?>(null));

        var act = async () => await _handler.Handle(
            new DeleteGenreCommand(Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<GenreNotFoundException>();
    }
}
