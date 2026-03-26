using FluentAssertions;
using Library.Application.Data;
using Library.Application.Services.Labels.Commands.CreateLabel;
using Library.ApplicationTests.Utils;
using Library.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.ApplicationTests.Labels;

public class CreateLabelHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly Mock<DbSet<Label>> _labelsDbSetMock;
    private readonly CreateLabelCommandHandler _handler;

    public CreateLabelHandlerTests()
    {
        _labelsDbSetMock = MockDbSetFactory.Create<Label>();

        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.Labels).Returns(_labelsDbSetMock.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _handler = new CreateLabelCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_AddsLabelAndReturnsId()
    {
        var command = new CreateLabelCommand("Nuclear Blast");

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Id.Should().NotBeEmpty();
        _labelsDbSetMock.Verify(x => x.Add(It.IsAny<Label>()), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesLabelWithCorrectName()
    {
        var command = new CreateLabelCommand("Peaceville Records");

        await _handler.Handle(command, CancellationToken.None);

        _labelsDbSetMock.Verify(x => x.Add(It.Is<Label>(l => l.Name == "Peaceville Records")), Times.Once);
    }
}
