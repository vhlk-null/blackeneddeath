using FluentAssertions;
using Library.Application.Data;
using Library.Application.Exceptions;
using Library.Application.Services.Labels.Commands.DeleteLabel;
using Library.ApplicationTests.Utils;
using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.ApplicationTests.Labels;

public class DeleteLabelHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly Mock<DbSet<Label>> _labelsDbSetMock;
    private readonly DeleteLabelCommandHandler _handler;

    public DeleteLabelHandlerTests()
    {
        _labelsDbSetMock = MockDbSetFactory.Create<Label>();

        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.Labels).Returns(_labelsDbSetMock.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _handler = new DeleteLabelCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingLabel_RemovesLabelAndReturnsSuccess()
    {
        Guid labelId = Guid.NewGuid();
        Label label = Label.Create(LabelId.Of(labelId), "Nuclear Blast");
        _labelsDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Label?>(label));

        DeleteLabelResult result = await _handler.Handle(new DeleteLabelCommand(labelId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _labelsDbSetMock.Verify(x => x.Remove(label), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingLabel_ThrowsLabelNotFoundException()
    {
        _labelsDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Label?>(null));

        Func<Task<DeleteLabelResult>> act = async () => await _handler.Handle(
            new DeleteLabelCommand(Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<LabelNotFoundException>();
    }
}
