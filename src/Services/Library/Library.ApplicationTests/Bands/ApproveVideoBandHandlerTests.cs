using FluentAssertions;
using Library.Application.Data;
using Library.Application.Exceptions;
using Library.Application.Services.Bands.Commands.ApproveVideoBand;
using Library.ApplicationTests.Utils;
using Library.Domain.Enums;
using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.ApplicationTests.Bands;

public class ApproveVideoBandHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly Mock<DbSet<VideoBand>> _videoBandsDbSetMock;
    private readonly ApproveVideoBandCommandHandler _handler;

    public ApproveVideoBandHandlerTests()
    {
        _videoBandsDbSetMock = MockDbSetFactory.Create<VideoBand>();
        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.VideoBands).Returns(_videoBandsDbSetMock.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _handler = new ApproveVideoBandCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingVideoBand_ApprovesAndReturnsSuccess()
    {
        Guid videoBandId = Guid.NewGuid();
        VideoBand videoBand = VideoBand.Create(BandId.Of(Guid.NewGuid()), "Death Live", null, null, VideoType.Live, "https://youtube.com/watch?v=abc", null, VideoBandId.Of(videoBandId));
        _videoBandsDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<VideoBand?>(videoBand));

        ApproveVideoBandResult result = await _handler.Handle(new ApproveVideoBandCommand(videoBandId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        videoBand.IsApproved.Should().BeTrue();
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingVideoBand_ThrowsVideoBandNotFoundException()
    {
        _videoBandsDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<VideoBand?>(null));

        Func<Task> act = async () => await _handler.Handle(new ApproveVideoBandCommand(Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<VideoBandNotFoundException>();
    }
}
