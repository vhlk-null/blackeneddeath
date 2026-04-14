using FluentAssertions;
using Library.Application.Data;
using Library.Application.Exceptions;
using Library.Application.Services.Labels.Commands.UpdateLabel;
using Library.ApplicationTests.Utils;
using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.ApplicationTests.Labels;

public class UpdateLabelHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly Mock<DbSet<Label>> _labelsDbSetMock;
    private readonly UpdateLabelCommandHandler _handler;

    public UpdateLabelHandlerTests()
    {
        _labelsDbSetMock = MockDbSetFactory.Create<Label>();
        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.Labels).Returns(_labelsDbSetMock.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _handler = new UpdateLabelCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingLabel_UpdatesNameAndReturnsSuccess()
    {
        Guid labelId = Guid.NewGuid();
        Label label = Label.Create(LabelId.Of(labelId), "Nuclear Blast");
        _labelsDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Label?>(label));

        UpdateLabelResult result = await _handler.Handle(
            new UpdateLabelCommand(labelId, "Century Media"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        label.Name.Should().Be("Century Media");
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingLabel_ThrowsLabelNotFoundException()
    {
        _labelsDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Label?>(null));

        Func<Task> act = async () => await _handler.Handle(
            new UpdateLabelCommand(Guid.NewGuid(), "Century Media"), CancellationToken.None);

        await act.Should().ThrowAsync<LabelNotFoundException>();
    }
}
