using FluentAssertions;
using Library.Application.Data;
using Library.Application.Exceptions;
using Library.Application.Services.Tags.Commands.UpdateTag;
using Library.ApplicationTests.Utils;
using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.ApplicationTests.Tags;

public class UpdateTagHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly Mock<DbSet<Tag>> _tagsDbSetMock;
    private readonly UpdateTagCommandHandler _handler;

    public UpdateTagHandlerTests()
    {
        _tagsDbSetMock = MockDbSetFactory.Create<Tag>();
        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.Tags).Returns(_tagsDbSetMock.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _handler = new UpdateTagCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingTag_UpdatesNameAndReturnsSuccess()
    {
        Guid tagId = Guid.NewGuid();
        Tag tag = Tag.Create(TagId.Of(tagId), "Melodic");
        _tagsDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Tag?>(tag));

        UpdateTagResult result = await _handler.Handle(
            new UpdateTagCommand(tagId, "Progressive"), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        tag.Name.Should().Be("Progressive");
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingTag_ThrowsTagNotFoundException()
    {
        _tagsDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Tag?>(null));

        Func<Task> act = async () => await _handler.Handle(
            new UpdateTagCommand(Guid.NewGuid(), "Progressive"), CancellationToken.None);

        await act.Should().ThrowAsync<TagNotFoundException>();
    }
}
