using FluentAssertions;
using Library.Application.Data;
using Library.Application.Services.Genres.Commands.CreateGenre;
using Library.ApplicationTests.Utils;
using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.ApplicationTests.Genres;

public class CreateGenreHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly Mock<DbSet<Genre>> _genresDbSetMock;
    private readonly CreateGenreCommandHandler _handler;

    public CreateGenreHandlerTests()
    {
        _genresDbSetMock = MockDbSetFactory.Create<Genre>();
        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.Genres).Returns(_genresDbSetMock.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _handler = new CreateGenreCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_AddsGenreAndReturnsId()
    {
        CreateGenreCommand command = new CreateGenreCommand("Death Metal", null);

        CreateGenreResult result = await _handler.Handle(command, CancellationToken.None);

        result.Id.Should().NotBeEmpty();
        _genresDbSetMock.Verify(x => x.Add(It.IsAny<Genre>()), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_CommandWithParentGenre_AddsGenreAndReturnsId()
    {
        Guid parentId = Guid.NewGuid();
        CreateGenreCommand command = new CreateGenreCommand("Old School Death Metal", parentId);

        CreateGenreResult result = await _handler.Handle(command, CancellationToken.None);

        result.Id.Should().NotBeEmpty();
        _genresDbSetMock.Verify(x => x.Add(It.IsAny<Genre>()), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
