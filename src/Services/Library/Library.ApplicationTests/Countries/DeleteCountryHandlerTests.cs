using FluentAssertions;
using Library.Application.Data;
using Library.Application.Exceptions;
using Library.Application.Services.Countries.Commands.DeleteCountry;
using Library.ApplicationTests.Utils;
using Library.Domain.Models;
using Library.Domain.ValueObjects.Ids;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace Library.ApplicationTests.Countries;

public class DeleteCountryHandlerTests
{
    private readonly Mock<ILibraryDbContext> _contextMock;
    private readonly Mock<DbSet<Country>> _countriesDbSetMock;
    private readonly DeleteCountryCommandHandler _handler;

    public DeleteCountryHandlerTests()
    {
        _countriesDbSetMock = MockDbSetFactory.Create<Country>();

        _contextMock = new Mock<ILibraryDbContext>();
        _contextMock.Setup(x => x.Countries).Returns(_countriesDbSetMock.Object);
        _contextMock.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        _handler = new DeleteCountryCommandHandler(_contextMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingCountry_RemovesCountryAndReturnsSuccess()
    {
        var countryId = Guid.NewGuid();
        var country = Country.Create(CountryId.Of(countryId), "Norway", "NO");
        _countriesDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Country?>(country));

        var result = await _handler.Handle(new DeleteCountryCommand(countryId), CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _countriesDbSetMock.Verify(x => x.Remove(country), Times.Once);
        _contextMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_NonExistingCountry_ThrowsCountryNotFoundException()
    {
        _countriesDbSetMock
            .Setup(x => x.FindAsync(It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(ValueTask.FromResult<Country?>(null));

        var act = async () => await _handler.Handle(
            new DeleteCountryCommand(Guid.NewGuid()), CancellationToken.None);

        await act.Should().ThrowAsync<CountryNotFoundException>();
    }
}
