using FluentAssertions;
using Library.Application.Data;
using Library.Application.Services.Tags.Commands.CreateTag;
using Library.ApplicationTests.Utils;
using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.ApplicationTests.Tags;

public class CreateTagHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly Mock<DbSet<Tag>> _tagsDbSetMock;
    private readonly CreateTagCommandHandler _handler;

    public CreateTagHandlerTests()
    {
        _tagsDbSetMock = MockDbSetFactory.Create<Tag>();

        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.Tags).Returns(_tagsDbSetMock.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _handler = new CreateTagCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_AddsTagAndReturnsId()
    {
        var command = new CreateTagCommand("Atmospheric");

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Id.Should().NotBeEmpty();
        _tagsDbSetMock.Verify(x => x.Add(It.IsAny<Tag>()), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesTagWithCorrectName()
    {
        var command = new CreateTagCommand("Progressive");

        await _handler.Handle(command, CancellationToken.None);

        _tagsDbSetMock.Verify(x => x.Add(It.Is<Tag>(t => t.Name == "Progressive")), Times.Once);
    }
}
