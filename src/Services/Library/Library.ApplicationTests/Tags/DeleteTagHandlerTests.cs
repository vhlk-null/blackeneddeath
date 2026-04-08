using FluentAssertions;
using Library.Application.Data;
using Library.Application.Exceptions;
using Library.Application.Services.Tags.Commands.DeleteTag;
using Library.ApplicationTests.Utils;
using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.ApplicationTests.Tags;

public class DeleteTagHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly Mock<DbSet<Tag>> _tagsDbSetMock;
    private readonly DeleteTagCommandHandler _handler;

    public DeleteTagHandlerTests()
    {
        _tagsDbSetMock = MockDbSetFactory.Create<Tag>();

        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.Tags).Returns(_tagsDbSetMock.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _handler = new DeleteTagCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingTag_RemovesTagAndReturnsSuccess()
    {
        Guid tagId = Guid.NewGuid();
        Tag tag = Tag.Create(TagId.Of(tagId), "Atmospheric");
        _tagsDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Tag?>(tag));

        DeleteTagResult result = await _handler.Handle(new DeleteTagCommand(tagId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _tagsDbSetMock.Verify(x => x.Remove(tag), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingTag_ThrowsTagNotFoundException()
    {
        _tagsDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Tag?>(null));

        Func<Task<DeleteTagResult>> act = async () => await _handler.Handle(
            new DeleteTagCommand(Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<TagNotFoundException>();
    }
}
